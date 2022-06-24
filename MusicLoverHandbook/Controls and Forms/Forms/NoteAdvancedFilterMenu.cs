using Microsoft.VisualBasic.Devices;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Sources;

namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    public partial class NoteAdvancedFilterMenu : Form
    {
        private List<NoteLite> originalLiteNotes;
        private List<NoteLite> previewFiltered;
        private Dictionary<
            StringTagTools.TagName,
            Dictionary<StringTagTools.TagValue, NoteLite[]>
        > taggedData = new();

        public NoteAdvancedFilterMenu(MainForm mainForm)
        {
            InitializeComponent();

            MainForm = mainForm;
            Font = MainForm.Font;

            SetupLayout();
        }

        public MainForm MainForm { get; }

        private List<NoteLite> PreviewFiltered
        {
            get => previewFiltered;
            set
            {
                previewFiltered = value;
                UpdatePreview();
            }
        }

        private NoteFilter CreateFilter() => new(byNameInput.Text, byDescInput.Text);

        private void DrawPreviewBorder()
        {
            Image back = new Bitmap(previewFilteredPanel.Width, previewFilteredPanel.Height);
            using (var g = Graphics.FromImage(back))
                g.DrawRectangle(
                    new Pen(ControlPaint.Dark(previewFilteredPanel.BackColor, 0.1f), 6),
                    0,
                    0,
                    back.Width,
                    back.Height
                );
            previewFilteredPanel.BackgroundImage = back;
        }

        private void InvokeFiltering()
        {
            PreviewFiltered = CreateFilter().ApplyOn(originalLiteNotes);
        }

        private void OnDescInputChanged(object? sender, EventArgs e) { }

        private void OnInputTextChanged(object? sender, EventArgs e)
        {
            InvokeFiltering();
        }
        private void SetupLayout()
        {
            titleLabel.BackColor = MainForm.LabelBackColor;
            titleContPanel.BackColor = MainForm.LabelBackColor;
            previewFilteredPanel.BackColor = ControlPaint.LightLight(MainForm.LabelBackColor);
            previewFilteredPanel.Padding = new Padding(6);
            previewFilteredPanel.Resize += (sender, e) =>
            {
                DrawPreviewBorder();
            };
            DrawPreviewBorder();
            noteTypeSelectFlow.BackColor = previewFilteredPanel.BackColor;
            actionsTable.BackColor = previewFilteredPanel.BackColor;
            BackColor = ControlPaint.LightLight(previewFilteredPanel.BackColor);

            originalLiteNotes = MainForm.NotesContainer.InnerNotes
                .SelectMany(x => x.Flatten())
                .ToList();
            originalLiteNotes.ForEach(x => x.Dock = DockStyle.Top);
            PreviewFiltered = originalLiteNotes;

            byNameInput.TextChanged += OnInputTextChanged;
            byDescInput.TextChanged += OnInputTextChanged;

            
        }

        private void UpdatePreview()
        {
            previewFilteredPanel.Controls.Clear();
            previewFilteredPanel.Controls.AddRange(PreviewFiltered.ToArray());
        }

        public class NoteFilter
        {
            public NoteFilter(string byName, string byDesc)
            {
                Name = byName.ToLower().Trim();
                DescComp =
                    byDesc != ""
                        ? byDesc.Split(";").Select(x => x.ToLower().Trim()).ToArray()
                        : new[] { "" };
            }

            public string[] DescComp { get; }
            public string Name { get; }

            public List<NoteLite> ApplyOn(List<NoteLite> lites)
{
                var filtered = lites.ToList();

                var nameOrCheck = Name.Replace(@"\|", "\n");
                if (nameOrCheck.Count(x => x == '|') == 0)
                    filtered = NameFilterize(filtered, nameOrCheck);
                else
                {
                    var forOr = nameOrCheck.Split('|').Select(x => x.Replace("\n", @"|").Trim());
                    var combFiltered = forOr.Select(x => NameFilterize(filtered, x)).Aggregate((c, n) => c.Concat(n).ToList());
                    filtered = combFiltered;
                }
                foreach (var compStr in DescComp)
                {
                    var descPartOrCheck = compStr.Replace(@"\|", "\n");
                    if (descPartOrCheck.Count(x => x == '|') == 0)
                        filtered = DescFilterize(filtered, compStr);
                    else
                    {
                        var forOr = descPartOrCheck.Split('|').Select(x=>x.Replace("\n",@"|").Trim());
                        var combFiltered = forOr.Select(x => DescFilterize(filtered, x)).Aggregate((c, n) => c.Concat(n).ToList());
                        filtered = combFiltered;
                    }
                }

                return filtered;
            }

            private List<NoteLite> DescFilterize(List<NoteLite> inputLites, string rawcomp)
            {
                var lites = inputLites.ToList();

                if (rawcomp == "")
                    return lites;
                bool exceptionMode = false;
                var comparer = rawcomp;

                if (rawcomp[0] == '!')
                {
                    exceptionMode = true;
                    comparer = rawcomp.Substring(1);
                }

                if (comparer == "")
                    return lites;

                if (comparer[0] != '#')
                {
                    var result = lites
                        .Where(x => x.Description.ToLower().Trim().Contains(comparer))
                        .ToList();
                    return exceptionMode ? lites.Except(result).ToList() : result;
                }
                else
                {
                    var data = lites
                        .SelectMany(
                            n =>
                                StringTagTools
                                    .GetTagged(n.Description, '#')
                                    .Select(x => (Name: x.Key, Value: x.Value, NoteLite: n))
                        )
                        .GroupBy(x => x.Name.GetHashCode())
                        .Select(x => x.GroupBy(d => d.Value))
                        .ToDictionary(
                            k => k.First().First().Name,
                            v =>
                                v.ToDictionary(
                                    k2 => k2.First().Value,
                                    v2 => v2.Select(x => x.NoteLite).ToArray()
                                )
                        );
                    var result = new List<NoteLite>();
                    if (data.Count == 0)
                    {
                        lites = exceptionMode ? lites.Except(result).ToList() : result;
                        return lites;
                    }
                    var tag = Regex.Match(comparer, @"[^\s]+").Value;
                    var value = string.Join(tag, comparer.Split(tag).Skip(1)).Trim().ToLower();
                    tag = tag.ToLower().Trim();
                    var query = data.Select(x => x);
                    if (tag == "#")
                        query = query.Select(x => x);
                    else
                        query = query.Where(
                            x =>
                                x.Key.ValueType == StringTagTools.TagDataType.Valued
                                && x.Key.Value!
                                    .ToLower()
                                    .Trim()
                                    .Contains(tag.Substring(1, tag.Length - 1))
                        );
                    if (query.Count() == 0)
                    {
                        lites = exceptionMode ? lites.Except(result).ToList() : result;
                        return lites;
                    }

                    var query2 = query.SelectMany(x => x.Value);
                    if (value == "")
                        query2 = query2.Select(x => x);
                    else
                    {
                        var splValue = value.Split(' ');
                        foreach (var subval in splValue)
                            query2 = query2.Where(
                                x =>
                                    x.Key.ValueType == StringTagTools.TagDataType.Valued
                                    && x.Key.Value!.ToLower().Trim().Contains(subval)
                            );
                    }
                    result = query2.SelectMany(x => x.Value).ToList();
                    return exceptionMode ? lites.Except(result).ToList() : result;

                }

            }

            private List<NoteLite> NameFilterize(List<NoteLite> lites, string nameCompare)
            {
                if (nameCompare == "")
                    return lites;
                var name = nameCompare;
                bool exceptionMode = false;
                if (nameCompare[0] == '!')
                {
                    name = nameCompare.Substring(1);
                    exceptionMode = true;
                }
                if (name == "")
                    return lites;
                var result = new List<NoteLite>();
                result = lites.Where(x => x.NoteName.ToLower().Trim().Contains(name)).ToList();
                return exceptionMode ? lites.Except(result).ToList() : result;
            }
        }
    }
}
