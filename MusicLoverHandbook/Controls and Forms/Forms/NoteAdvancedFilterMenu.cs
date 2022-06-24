using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    public partial class NoteAdvancedFilterMenu : Form
    {
        private List<NoteLite> previewFiltered;

        private List<NoteLite> originalLiteNotes;

        private Dictionary<string, string[]> taggedData;

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

        private void OnInputTextChanged(object? sender, EventArgs e)
        {
            InvokeFiltering();
        }

        private NoteFilter CreateFilter() => new(byNameInput.Text, byDescInput.Text);

        private void InvokeFiltering()
        {
            PreviewFiltered = CreateFilter().ApplyOn(originalLiteNotes);
        }

        private void OnDescInputChanged(object? sender, EventArgs e) { }

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

            taggedData = originalLiteNotes
                .SelectMany(x => StringTools.GetTagged(x.Description, '#'))
                .GroupBy(x => x.Tag)
                .ToDictionary(k => k.Key, v => v.Select(x => x.Data).Distinct().ToArray());

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
                return DescFilterize(NameFilterize(lites));
            }

            private List<NoteLite> DescFilterize(List<NoteLite> lites)
            {
                foreach (var comp in DescComp)
                {
                    if (comp == "")
                        continue;
                    var s = comp;
                    if (comp[0] == '#')
                        s = StringTools.GetTagData(comp);
                    Debug.WriteLine("COMPAER: " + s);
                    if (comp != "")
                        lites = lites
                            .Where(x => x.Description.ToLower().Trim().Contains(s))
                            .ToList();
                }
                return lites;
            }

            private List<NoteLite> NameFilterize(List<NoteLite> lites)
            {
                if (Name == "")
                    return lites;
                return lites.Where(x => x.NoteName.ToLower().Trim().Contains(Name)).ToList();
            }
        }
    }
}
