﻿using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Logic.Notes;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Data;
using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    public partial class NoteAdvancedFilterMenu : Form
    {
        public List<INoteControlChild> FinalizedOutput = new();
        private BasicFilterResultsChangeEventHandler? basicFilteringResultsChange;
        private List<BasicSwitchLabel> currentFilteredSwitch = new();
        private List<NoteLite> filteredNotesSwitchless;
        private List<NoteLite> initialNotes;
        private int previewUpdatingCooldown = 100;
        private Timer previewUpdatingTimer;
        private BasicSwitchLabel smartFilterIncludeSwitch;
        private List<SmartFilteringOptionMenu> smartFilterOptions = new();
        private BasicSwitchLabel SSLNSwitch;

        private Dictionary<
            StringTagTools.TagName,
            Dictionary<StringTagTools.TagValue, NoteLite[]>
        > taggedInformation = new();

        public MainForm MainForm { get; }

        private List<NoteLite> FilteredNotesFinal
        {
            get =>
                (
                    from switchlessNote in FilteredNotesSwitchless
                    where
                        currentFilteredSwitch
                            .Find(s => (NoteType)s.Tag == switchlessNote.Ref.NoteType)
                            ?.SpecialState ?? true
                    select switchlessNote
                ).ToList();
        }

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

        public NoteAdvancedFilterMenu(MainForm mainForm)
        {
            InitializeComponent();

            MainForm = mainForm;
            Font = MainForm.Font;

            SetupLayout();
        }

        public void ApplyFilterButton_Click(object? sender, EventArgs e)
        {
            Finalization();
        }

        public void DelayedPreviewUpdate()
        {
            previewUpdatingCooldown = 40;
        }

        public void UpdatePreview()
        {
            previewFilteredPanel.SuspendLayout();
            var control = previewFilteredPanel.Controls;

            while (control.Count > 0)
                control.RemoveAt(0);

            foreach (var ctrl in FilteredNotesFinal)
                control.Add(ctrl);
            previewFilteredPanel.ResumeLayout();
            OnBasicFilteringResultsChange();
        }

        private NoteLiteFilter CreateBasicFilter() => new(byNameInput.Text, byDescInput.Text);

        private List<INoteControlChild> CreateFinalizedOutput()
        {
            if (!smartFilterIncludeSwitch.SpecialState)
            {
                var parentless =
                    from filteredNote in FilteredNotesFinal
                    where
                        filteredNote.Ref is INoteControlChild child
                        && GetFirstIncludedInFinal(
                            new((from par in GetParents(child) select par).Reverse().ToList())
                        ) == null
                    select filteredNote;

                if (SSLNSwitch.SpecialState)
                    return (
                        from parentlessNote in parentless
                        select parentlessNote.Ref.Clone() into parentlessClone
                        where parentlessClone is INoteControlChild
                        select (INoteControlChild)parentlessClone
                    ).ToList();
                else
                {
                    var included =
                        from lite in parentless
                        select (
                            Head: lite.Ref.Clone(),
                            LiteFind: from flattened in lite.Ref.Flatten()
                            where FilteredNotesFinal.Any(f => f.Equals(flattened) && f != flattened)
                            select flattened
                        );

                    var output = new List<INoteControlChild>();
                    foreach (var inc in included)
                    {
                        if (inc.Head is not INoteControlChild asChild)
                            continue;
                        if (inc.Head is INoteControlParent asParent)
                        {
                            SmartOccurancesLeaver(asParent, inc.LiteFind);
                        }
                        ;
                        output.Add((INoteControlChild)inc.Head);
                    }
                    return output;
                }
            }
            else
            {
                List<INoteControlChild> retValues = new();
                foreach (var smartFilter in smartFilterOptions)
                {
                    var loadedParents = new List<IParentControl>();
                    if (smartFilter.CurrentlySelectedTypeOption is null)
                        continue;

                    NoteType toFindType = (NoteType)smartFilter.CurrentlySelectedTypeOption;
                    var ssln = smartFilter.SSLNSwitch.SpecialState;

                    foreach (var note in smartFilter.OneTypeNotes)
                    {
                        if (
                            note.Ref is INoteControlChild noteRefAsChild
                            && GetParents(noteRefAsChild) is var noteRefParents
                            && noteRefParents.FindIndex(
                                noteParent =>
                                    noteParent is INoteControlParent controlParent
                                    && controlParent.NoteType == toFindType
                            )
                                is int parentInd
                            && parentInd > -1
                        )
                        {
                            var affectedParents = new ArraySegment<IParentControl>(
                                noteRefParents.ToArray(),
                                0,
                                parentInd + 1
                            );
                            if (loadedParents.Any(x => affectedParents.Contains(x)))
                                continue;

                            loadedParents.AddRange(affectedParents);
                            var parent = (INoteControlParent)noteRefParents[parentInd];
                            parent = (INoteControlParent)parent.Clone();
                            if (!ssln)
                                SmartOccurancesLeaver(parent, smartFilter.OneTypeNotes);
                            retValues.Add((INoteControlChild)parent);
                        }
                        else if (
                            note.Ref is INoteControlParent asParent
                            && asParent.Flatten().FindAll(x => x.NoteType == toFindType)
                                is List<NoteLite> { Count: > 0 } childResults
                        )
                        {
                            retValues.AddRange(
                                (
                                    from child in childResults
                                    select child.Ref.Clone()
                                ).OfType<INoteControlChild>()
                            );
                        }
                    }
                }
                return retValues;
            }
        }

        private IEnumerable<BasicSwitchLabel> CreateSwitchButtons()
        {
            var types = FilteredNotesSwitchless.Select(x => x.Ref.NoteType).Distinct();
            foreach (var type in types)
            {
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
            FilteredNotesSwitchless = CreateBasicFilter().ApplyOn(initialNotes).ToList();
            OnBasicFilteringResultsChange();
        }

        private void Finalization()
        {
            FinalizedOutput = CreateFinalizedOutput();
            DialogResult = DialogResult.OK;
            Close();
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

        private List<IParentControl> GetParents(INoteControlChild child)
        {
            return new List<IParentControl>() { child.ParentNote }
                .Concat(child.ParentNote is INoteControlChild inchild ? GetParents(inchild) : new())
                .ToList();
        }

        private void InvokeFiltering()
        {
            Filtering();
            UpdateSwitchButtons();
        }

        private void OnBasicFilteringResultsChange()
        {
            if (basicFilteringResultsChange != null)
                basicFilteringResultsChange(FilteredNotesFinal);

            var advancedWorkWith = FilteredNotesFinal
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
            advFiltersFlow.Controls.Clear();
            advFiltersFlow.Controls.AddRange(smartFilterOptions.ToArray());
        }

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
                        UpdatePreview();
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

            applyFilterButton.Click += ApplyFilterButton_Click;

            InvokeFiltering();
        }

        private bool SmartOccurancesLeaver(
            INoteControlParent parent,
            IEnumerable<NoteLite> included,
            NoteType? stopClearingOn = null
        )
        {
            bool usefull = false;

            foreach (var child in parent.InnerNotes.ToList())
            {
                if (included.Any(x => child.RoughEquals(x)))
                {
                    usefull = true;
                    if (child is INoteControlParent asParent)
                        SmartOccurancesLeaver(asParent, included);
                }
                else
                {
                    if (child is INoteControlParent asParent)
                    {
                        if (
                            stopClearingOn != child.NoteType
                            && !SmartOccurancesLeaver(asParent, included, stopClearingOn)
                        )
                        {
                            parent.InnerNotes.Remove(child);
                        }
                        else
                            usefull = true;
                    }
                    else { }
                }
            }
            return usefull;
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
        }

        public delegate void BasicFilterResultsChangeEventHandler(
            List<NoteLite> prefilteredResults
        );

        public event BasicFilterResultsChangeEventHandler BasicFilterResultsChange
        {
            add => basicFilteringResultsChange += value;
            remove => basicFilteringResultsChange -= value;
        }
    }
}
