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
        private List<BasicSwitchLabel> currentFilteredSwitch = new();
        private List<NoteLite> initialNotes;
        private BasicFilterResultsChangeEventHandler? basicFilteringResultsChange;
        private List<NoteLite> filteredNotesSwitchless;
        private int previewUpdatingCooldown = 100;
        private System.Windows.Forms.Timer previewUpdatingTimer;
        private BasicSwitchLabel smartFilterIncludeSwitch;
        private BasicSwitchLabel SSLNSwitch;

        private Dictionary<
            StringTagTools.TagName,
            Dictionary<StringTagTools.TagValue, NoteLite[]>
        > taggedInformation = new();

        public MainForm MainForm { get; }

        private List<NoteLite> FilteredNotesSwitchless
        {
            get => filteredNotesSwitchless;
            set
            {
                var cloned = value.Select(x => x.Clone()).ToList();
                cloned.ForEach(x => x.Dock = DockStyle.Top);
                filteredNotesSwitchless = cloned;
                DelayedPreviewUpdate();
            }
        }

        private List<NoteLite> FilteredNotesFinal
        {
            get =>
                FilteredNotesSwitchless
                    .Where(
                        x =>
                            currentFilteredSwitch
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
            foreach (var ctrl in FilteredNotesFinal)
                control.Add(ctrl);
            Debug.WriteLine("Update Preview add end");
            previewFilteredPanel.ResumeLayout();
        }

        public void DelayedPreviewUpdate()
        {
            previewUpdatingCooldown = 40;
        }

        private NoteFilter CreateBasicFilter() => new(byNameInput.Text, byDescInput.Text);

        private List<INoteControlChild> CreateFinalizedOutput()
        {
            if (!smartFilterIncludeSwitch.SpecialState)
            {
                var parentless = FilteredNotesFinal.Where(
                    x =>
                        x.Ref is INoteControlChild child
                        && GetFirstIncludedInFinal(
                            new(GetParents(child).Select(x => x).Reverse().ToList())
                        ) == null
                );
                if (SSLNSwitch.SpecialState)
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
                                    .Where(s => FilteredNotesFinal.Any(k => k.Equals(s) && k != x))
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
                            SmartOccurancesLeaver(asParent, inc.LiteFind);
                            Debug.WriteLine("OCCURANCES FILTERING END");
                        }
                        ;
                        output.Add((INoteControlChild)inc.Head);
                    }
                    foreach (var o in output)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine(o.ToString());
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
            var types = FilteredNotesSwitchless.Select(x => x.Ref.NoteType).Distinct();
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
            FilteredNotesSwitchless = CreateBasicFilter().ApplyOn(initialNotes).ToList();
            Debug.WriteLine("Filtering raw filter ready");
            Debug.WriteLine("Calling on onPreFilteredChanged");
            OnBasicFilteringResultsChange();
        }

        private void Finalization()
        {
            FinalizedOutput = CreateFinalizedOutput();
        }

        private List<IParentControl> GetParents(INoteControlChild child)
        {
            return new List<IParentControl>() { child.ParentNote }
                .Concat(child.ParentNote is INoteControlChild inchild ? GetParents(inchild) : new())
                .ToList();
        }

        private NoteLite? GetFirstIncludedInFinal(LinkedList<IParentControl> parents)
        {
            for (var note = parents.First; note != null; note = note.Next)
            {
                if (FilteredNotesFinal.Find(x => x.Ref == note.Value) is NoteLite lite)
                    return lite;
            }
            return null;
        }

        private void InvokeFiltering()
        {
            Filtering();
            Debug.WriteLine("Calling updating switches");
            UpdateSwitchButtons();
        }

        private bool SmartOccurancesLeaver(INoteControlParent parent, List<NoteLite> included)
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
                        SmartOccurancesLeaver(asParent, included);
                }
                else
                {
                    Debug.WriteLine("Not included name: " + child.NoteName + " " + child.NoteType);
                    if (child is INoteControlParent asParent)
                    {
                        Debug.WriteLine(
                            "Not inc parent name: " + child.NoteName + " " + child.NoteType
                        );

                        if (!SmartOccurancesLeaver(asParent, included))
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

        private void OnInputTextChanged(object? sender, EventArgs e)
        {
            Debug.WriteLine("On input changed");
            InvokeFiltering();
        }

        private void OnBasicFilteringResultsChange()
        {
            Debug.WriteLine("OnPreFilteredResultChanges invoking start");
            if (basicFilteringResultsChange != null)
                basicFilteringResultsChange(FilteredNotesSwitchless);

            var advancedWorkWith = FilteredNotesSwitchless
                .GroupBy(x => x.Ref.NoteType)
                .Select(x => x.ToArray());

            smartFilterOptions = new List<SmartFilteringOptionMenu>();
            foreach (var oneTyped in advancedWorkWith)
                smartFilterOptions.Add(
                    new SmartFilteringOptionMenu(this, oneTyped)
                    {
                        Size = new(200, advFiltersFlow.Height),
                    }
                );
            Debug.WriteLine("Clearing smart filters");
            advFiltersFlow.Controls.Clear();
            advFiltersFlow.Controls.AddRange(smartFilterOptions.ToArray());
            Debug.WriteLine("Smart filters added");
        }

        private List<SmartFilteringOptionMenu> smartFilterOptions = new();

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

            previewUpdatingTimer = new System.Windows.Forms.Timer()
            {
                Interval = 1,
                Enabled = true,
            };
            previewUpdatingTimer.Tick += (sender, e) =>
            {
                if (previewUpdatingCooldown >= 0)
                {
                    if (previewUpdatingCooldown == 0)
                    {
                        Debug.WriteLine("Timed update start");
                        UpdatePreview();
                        Debug.WriteLine("Timed update end");
                    }
                    previewUpdatingCooldown--;
                }
                ;
            };

            noteTypeSelectFlow.BackColor = previewFilteredPanel.BackColor;
            advFiltersFlow.BackColor = previewFilteredPanel.BackColor;
            BackColor = ControlPaint.LightLight(previewFilteredPanel.BackColor);

            initialNotes = MainForm.NotesContainer.InnerNotes.SelectMany(x => x.Flatten()).ToList();
            initialNotes.ForEach(x => x.Dock = DockStyle.Top);

            FilteredNotesSwitchless = initialNotes;

            byNameInput.TextChanged += OnInputTextChanged;
            byDescInput.TextChanged += OnInputTextChanged;

            smartFilterIncludeSwitch = new(Color.LightGray, Color.DeepSkyBlue, false)
            {
                Text = "Smart filters",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            smartFiltersTable.Controls.Add(smartFilterIncludeSwitch, 1, 0);
            smartFilterIncludeSwitch.SpecialStateChanged += (sender, state) =>
            {
                advFiltersFlow.Visible = state;
            };
            SSLNSwitch = new(Color.OrangeRed, Color.LightGreen, true)
            {
                Text = "S.S.L.N",
                BasicTooltipText = "Save Same-Level Notes (NO)",
                SpecialTooltipText = "Save Same-Level Notes (YES)",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            filteringTable.Controls.Add(SSLNSwitch, 0, 1);

            applyFilterButton.Click += OnApplyFilteringButtonClick;

            InvokeFiltering();
        }

        private void UpdateSwitchButtons()
        {
            noteTypeSelectFlow.Controls.Clear();
            var buttons = CreateSwitchButtons().ToArray();
            foreach (var button in buttons)
            {
                button.SpecialStateChanged += (sender, state) =>
                {
                    DelayedPreviewUpdate();
                };
            }

            noteTypeSelectFlow.Controls.AddRange(buttons);
            buttons
                .ToList()
                .ForEach(
                    x =>
                        x.SpecialState =
                            currentFilteredSwitch
                                .Find(f => (NoteType)f.Tag == (NoteType)x.Tag)
                                ?.SpecialState ?? x.SpecialState
                );
            currentFilteredSwitch = buttons.ToList();
            Debug.WriteLine($"Switch buttons ready: {currentFilteredSwitch.Count}");
        }

        public delegate void BasicFilterResultsChangeEventHandler(
            List<NoteLite> prefilteredResults
        );

        public event BasicFilterResultsChangeEventHandler BasicFilterResultsChange
        {
            add => basicFilteringResultsChange += value;
            remove => basicFilteringResultsChange -= value;
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
                    filtered = NameFiltering(filtered, nameOrCheck);
                else
                {
                    var forOr = nameOrCheck.Split('|').Select(x => x.Replace("\n", @"|").Trim());
                    var combFiltered = forOr
                        .Select(x => NameFiltering(filtered, x))
                        .Aggregate((c, n) => c.Concat(n).ToList());
                    filtered = combFiltered;
                }
                foreach (var compStr in DescComp)
                {
                    var descPartOrCheck = compStr.Replace(@"\|", "\n");
                    if (descPartOrCheck.Count(x => x == '|') == 0)
                        filtered = DescFiltering(filtered, compStr);
                    else
                    {
                        var forOr = descPartOrCheck
                            .Split('|')
                            .Select(x => x.Replace("\n", @"|").Trim());
                        var combFiltered = forOr
                            .Select(x => DescFiltering(filtered, x))
                            .Aggregate((c, n) => c.Concat(n).ToList());
                        filtered = combFiltered;
                    }
                }

                return filtered;
            }

            private List<NoteLite> DescFiltering(List<NoteLite> inputLites, string rawcomp)
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

            private List<NoteLite> NameFiltering(List<NoteLite> lites, string nameCompare)
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
