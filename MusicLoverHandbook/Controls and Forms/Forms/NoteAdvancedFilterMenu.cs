using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    public partial class NoteAdvancedFilterMenu : Form
    {
        public List<INoteControlChild> FinalizedOutput = new();
        private List<BasicSwitchLabel> currentButtons = new();
        private List<NoteLite> originalLiteNotes;
        private PreFilteringResultChangedEventHandler? preFilteringResultChanged;
        private List<NoteLite> previewFiltered;
        private int previewUpdateCooldown = 100;
        private System.Windows.Forms.Timer previewUpdateTimer;
        private BasicSwitchLabel smartFiltersSwitch;
        private BasicSwitchLabel sslnSwitch;

        private Dictionary<
            StringTagTools.TagName,
            Dictionary<StringTagTools.TagValue, NoteLite[]>
        > taggedData = new();

        public MainForm MainForm { get; }

        private List<NoteLite> NoteLiteFilteredExceptButtons
        {
            get => previewFiltered;
            set
            {
                var cloned = value.Select(x => x.Clone()).ToList();
                cloned.ForEach(x => x.Dock = DockStyle.Top);
                previewFiltered = cloned;
                UpdatePreviewDelayed();
            }
        }

        private List<NoteLite> NoteLiteFilteredFull
        {
            get =>
                NoteLiteFilteredExceptButtons
                    .Where(
                        x =>
                            currentButtons
                                .Find(f => (NoteType)f.Tag == x.Ref.NoteType)
                                ?.SpecialState ?? true
                    )
                    .ToList();
        }

        public NoteAdvancedFilterMenu(MainForm mainForm)
        {
            InitializeComponent();

            MainForm = mainForm;
            Font = MainForm.Font;

            SetupLayout();
        }

        public void OnApplyFilteringButtonClick(object? sender, EventArgs e)
        {
            Finalization();
        }

        public void UpdatePreview()
        {
            previewFilteredPanel.SuspendLayout();
            Debug.WriteLine("Update Preview Clear start");
            var control = previewFilteredPanel.Controls;

            while (control.Count > 0)
                control.RemoveAt(0);
            Debug.WriteLine("Update Preview Clear end");

            Debug.WriteLine("Update Preview add start");
            //previewFilteredPanel.Controls.AddRange(NoteLiteFilteredBase.ToArray());
            foreach (var ctrl in NoteLiteFilteredFull)
                control.Add(ctrl);
            Debug.WriteLine("Update Preview add end");
            previewFilteredPanel.ResumeLayout();
        }

        public void UpdatePreviewDelayed()
        {
            previewUpdateCooldown = 40;
        }

        private NoteFilter CreateFilter() => new(byNameInput.Text, byDescInput.Text);

        private List<INoteControlChild> CreateFinalizedOutput()
        {
            if (!smartFiltersSwitch.SpecialState)
            {
                var parentless = NoteLiteFilteredFull.Where(
                    x =>
                        x.Ref is INoteControlChild child
                        && GetMostIncluded(
                            new(GetAllParents(child).Select(x => x).Reverse().ToList())
                        ) == null
                );
                if (sslnSwitch.SpecialState)
                    return parentless
                        .Select(x => x.Ref.Clone())
                        .Where(x => x is INoteControlChild)
                        .Cast<INoteControlChild>()
                        .ToList();
                else
                {
                    var included = parentless.Select(
                        x =>
                            (
                                Head: x.Ref.Clone(),
                                LiteFind: x.Ref
                                    .Flatten()
                                    .Where(
                                        s => NoteLiteFilteredFull.Any(k => k.Equals(s) && k != x)
                                    )
                                    .ToList()
                            )
                    );
                    var output = new List<INoteControlChild>();
                    Debug.WriteLine(included.Count());
                    foreach (var inc in included)
                    {
                        if (inc.Head is not INoteControlChild asChild)
                            continue;
                        if (inc.Head is INoteControlParent asParent)
                        {
                            Debug.WriteLine("OCCURANCES FILTERING BEGIN");
                            Debug.WriteLine("");
                            LeaveOccurances(asParent, inc.LiteFind);
                            Debug.WriteLine("");
                            Debug.WriteLine("OCCURANCES FILTERING END");
                        }
                        ;
                        output.Add((INoteControlChild)inc.Head);
                    }
                    foreach (var o in output)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine("");
                        Debug.WriteLine(o.ToString());
                        Debug.WriteLine("");
                        Debug.WriteLine("");
                    }
                    return output;
                }
            }
            else { }
            throw new NotImplementedException();
        }

        private IEnumerable<BasicSwitchLabel> CreateSwitchButtons()
        {
            var types = NoteLiteFilteredExceptButtons.Select(x => x.Ref.NoteType).Distinct();
            foreach (var type in types)
            {
                Debug.WriteLine($"Creating note enabler button type : {type}");
                Size bSize = new(noteTypeSelectFlow.Width / 8, noteTypeSelectFlow.Height);
                var basic = new BasicSwitchLabel(
                    Color.LightGray,
                    type.GetLiteColor() ?? type.GetColor() ?? Color.LightGreen,
                    true
                )
                {
                    Text = type.ToString(true),
                    Size = bSize,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BasicTooltipText = $"{type.ToString(true)} type isn't included",
                    SpecialTooltipText = $"{type.ToString(true)} type included",
                    SwitchType = BasicSwitchLabel.SwitchMode.Click,
                    Tag = type,
                };

                yield return basic;
            }
        }

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

        private void Filtering()
        {
            Debug.WriteLine("Invoking filtering");
            NoteLiteFilteredExceptButtons = CreateFilter().ApplyOn(originalLiteNotes).ToList();
            Debug.WriteLine("Filtering raw filter ready");
            Debug.WriteLine("Calling on onPreFilteredChanged");
            OnPreFilteredResultChanged();
        }

        private void Finalization()
        {
            FinalizedOutput = CreateFinalizedOutput();
        }

        private List<IParentControl> GetAllParents(INoteControlChild child)
        {
            return new List<IParentControl>() { child.ParentNote }
                .Concat(
                    child.ParentNote is INoteControlChild inchild ? GetAllParents(inchild) : new()
                )
                .ToList();
        }

        private NoteLite? GetMostIncluded(LinkedList<IParentControl> parents)
        {
            for (var note = parents.First; note != null; note = note.Next)
            {
                if (NoteLiteFilteredFull.Find(x => x.Ref == note.Value) is NoteLite lite)
                    return lite;
            }
            return null;
        }

        private void InputsChangedFiltering()
        {
            Filtering();
            Debug.WriteLine("Calling updating switches");
            UpdateSwitchButtons();
        }

        private bool LeaveOccurances(INoteControlParent parent, List<NoteLite> included)
        {
            bool usefull = false;
            Debug.WriteLine("");

            foreach (var child in parent.InnerNotes.ToList())
            {
                Debug.WriteLine("Begin check name: " + child.NoteName + " " + child.NoteType);
                if (included.Any(x => child.RoughEquals(x)))
                {
                    Debug.WriteLine("Included name: " + child.NoteName + " " + child.NoteType);
                    usefull = true;
                    if (child is INoteControlParent asParent)
                        LeaveOccurances(asParent, included);
                }
                else
                {
                    Debug.WriteLine("Not included name: " + child.NoteName + " " + child.NoteType);
                    if (child is INoteControlParent asParent)
                    {
                        Debug.WriteLine(
                            "Not inc parent name: " + child.NoteName + " " + child.NoteType
                        );

                        if (!LeaveOccurances(asParent, included))
                        {
                            Debug.WriteLine(
                                "Parent left useless: " + child.NoteName + " " + child.NoteType
                            );
                            parent.InnerNotes.Remove(child);
                        }
                        else
                            usefull = true;
                    }
                    else
                    {
                        Debug.WriteLine(
                            "Not inc useless name: " + child.NoteName + " " + child.NoteType
                        );
                        parent.InnerNotes.Remove(child);
                    }
                }
            }
            return usefull;
        }

        private void OnDescInputChanged(object? sender, EventArgs e) { }

        private void OnInputTextChanged(object? sender, EventArgs e)
        {
            Debug.WriteLine("On input changed");
            InputsChangedFiltering();
        }

        private void OnPreFilteredResultChanged()
        {
            Debug.WriteLine("OnPreFilteredResultChanges invoking start");
            if (preFilteringResultChanged != null)
                preFilteringResultChanged(NoteLiteFilteredExceptButtons);

            var advancedWorkWith = NoteLiteFilteredExceptButtons
                .GroupBy(x => x.Ref.NoteType)
                .Select(x => x.ToArray());
            var filters = new List<FilteringControl>();

            foreach (var oneTyped in advancedWorkWith)
            {
                var advFilt = new FilteringControl(this, oneTyped)
                {
                    Size = new(200, advFiltersFlow.Height),
                };
                filters.Add(advFilt);
            }
            ;
            Debug.WriteLine("Clearing adv filters");
            advFiltersFlow.Controls.Clear();
            advFiltersFlow.Controls.AddRange(filters.ToArray());
            Debug.WriteLine("Adv filters added");
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

            previewUpdateTimer = new System.Windows.Forms.Timer() { Interval = 1, Enabled = true, };
            previewUpdateTimer.Tick += (sender, e) =>
            {
                if (previewUpdateCooldown >= 0)
                {
                    if (previewUpdateCooldown == 0)
                    {
                        Debug.WriteLine("Timed update start");
                        UpdatePreview();
                        Debug.WriteLine("Timed update end");
                    }
                    previewUpdateCooldown--;
                }
                ;
            };

            noteTypeSelectFlow.BackColor = previewFilteredPanel.BackColor;
            advFiltersFlow.BackColor = previewFilteredPanel.BackColor;
            BackColor = ControlPaint.LightLight(previewFilteredPanel.BackColor);

            originalLiteNotes = MainForm.NotesContainer.InnerNotes
                .SelectMany(x => x.Flatten())
                .ToList();
            originalLiteNotes.ForEach(x => x.Dock = DockStyle.Top);

            NoteLiteFilteredExceptButtons = originalLiteNotes;

            byNameInput.TextChanged += OnInputTextChanged;
            byDescInput.TextChanged += OnInputTextChanged;

            smartFiltersSwitch = new(Color.LightGray, Color.DeepSkyBlue, false)
            {
                Text = "Smart filters",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            smartFiltersTable.Controls.Add(smartFiltersSwitch, 1, 0);
            smartFiltersSwitch.SpecialStateChanged += (sender, state) =>
            {
                advFiltersFlow.Visible = state;
            };
            sslnSwitch = new(Color.OrangeRed, Color.LightGreen, true)
            {
                Text = "S.S.L.N",
                BasicTooltipText = "Save Same-Level Notes (NO)",
                SpecialTooltipText = "Save Same-Level Notes (YES)",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            filteringTable.Controls.Add(sslnSwitch, 0, 1);

            applyFilterButton.Click += OnApplyFilteringButtonClick;

            InputsChangedFiltering();
        }

        private void UpdateSwitchButtons()
        {
            noteTypeSelectFlow.Controls.Clear();
            var buttons = CreateSwitchButtons().ToArray();
            foreach (var button in buttons)
            {
                button.SpecialStateChanged += (sender, state) =>
                {
                    UpdatePreviewDelayed();
                };
            }

            noteTypeSelectFlow.Controls.AddRange(buttons);
            buttons
                .ToList()
                .ForEach(
                    x =>
                        x.SpecialState =
                            currentButtons
                                .Find(f => (NoteType)f.Tag == (NoteType)x.Tag)
                                ?.SpecialState ?? x.SpecialState
                );
            currentButtons = buttons.ToList();
            Debug.WriteLine($"Switch buttons ready: {currentButtons.Count}");
        }

        public delegate void PreFilteringResultChangedEventHandler(
            List<NoteLite> prefilteredResults
        );

        public event PreFilteringResultChangedEventHandler PreFilteringResultChanged
        {
            add => preFilteringResultChanged += value;
            remove => preFilteringResultChanged -= value;
        }

        public class NoteFilter
        {
            public string[] DescComp { get; }

            public string Name { get; }

            public NoteFilter(string byName, string byDesc)
            {
                Name = byName.ToLower().Trim();
                DescComp =
                    byDesc != ""
                        ? byDesc.Split(";").Select(x => x.ToLower().Trim()).ToArray()
                        : new[] { "" };
            }

            public List<NoteLite> ApplyOn(List<NoteLite> lites)
            {
                var filtered = lites.ToList();

                var nameOrCheck = Name.Replace(@"\|", "\n");
                if (nameOrCheck.Count(x => x == '|') == 0)
                    filtered = NameFilterize(filtered, nameOrCheck);
                else
                {
                    var forOr = nameOrCheck.Split('|').Select(x => x.Replace("\n", @"|").Trim());
                    var combFiltered = forOr
                        .Select(x => NameFilterize(filtered, x))
                        .Aggregate((c, n) => c.Concat(n).ToList());
                    filtered = combFiltered;
                }
                foreach (var compStr in DescComp)
                {
                    var descPartOrCheck = compStr.Replace(@"\|", "\n");
                    if (descPartOrCheck.Count(x => x == '|') == 0)
                        filtered = DescFilterize(filtered, compStr);
                    else
                    {
                        var forOr = descPartOrCheck
                            .Split('|')
                            .Select(x => x.Replace("\n", @"|").Trim());
                        var combFiltered = forOr
                            .Select(x => DescFilterize(filtered, x))
                            .Aggregate((c, n) => c.Concat(n).ToList());
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
