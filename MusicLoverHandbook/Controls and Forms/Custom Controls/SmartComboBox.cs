﻿using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Diagnostics;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class SmartComboBox : ComboBox, ISmartComboBox
    {
        public bool CanBeEmpty = false;

        public string? DefaultReplacement;

        private InputStatus state;

        private StateChangedEvent? statusChanged;

        private StateChangedEvent? statusChangedRepeatedly;

        private StateChangedEvent? tempStatusChangedRepeatedly;

        private ToolTip tooltip = new ToolTip() { InitialDelay = 0, IsBalloon = true, };

        public SmartComboBox()
        {
            TextChanged += OnInputDetected;
            LostFocus += OnLostFocus;
        }

        public delegate void StateChangedEvent(SmartComboBox sender, InputStatus state);

        public event StateChangedEvent StatusChanged
        {
            add => statusChanged += value;
            remove => statusChanged -= value;
        }

        public event StateChangedEvent StatusChangedRepeatedly
        {
            add => statusChangedRepeatedly += value;
            remove => statusChangedRepeatedly -= value;
        }

        public event StateChangedEvent TempStatusChangedRepeatedly
        {
            add => tempStatusChangedRepeatedly += value;
            remove => tempStatusChangedRepeatedly -= value;
        }

        public List<NoteControl> InnerData =>
            (
                NoteParent?.InnerNotes.Where(x => x is NoteControl).Cast<NoteControl>().ToList()
                ?? NotesContainer
                    ?.InnerNotes.Where(x => x is NoteControl)
                    .Cast<NoteControl>()
                    .ToList()
                ?? new()
            )
                .Where(
                    x =>
                        x.GetType().IsSubclassOf(RestrictedType)
                        || x.GetType().Equals(RestrictedType)
                )
                .ToList();

        public InputType InputType { get; set; }
        public NoteControlParent? NoteParent { get; set; }
        public NotesContainer? NotesContainer { get; set; }
        public Type RestrictedType { get; set; } = typeof(object);

        public InputStatus Status
        {
            get => state;
            set
            {
                if (state == value)
                {
                    OnStatusChangedRepeatedly();
                    return;
                }
                BackColor = Color.FromArgb(255, Color.FromArgb((int)value));
                state = value;
                Debug.WriteLine($"Change State to {value} is {this}");
                OnStatusChanged();
                ToggleActivity();
                SetToolTip();
            }
        }

        public void CheckValid()
        {
            CheckText();
            if (!CanBeEmpty && Status.IsError())
                Text = DefaultReplacement ?? $"Unknown {InputType.ToString() ?? "???"}";
        }

        public void ClearDataSource()
        {
            RestrictedType = typeof(object);
            Items.Clear();
            NotesContainer = null;
            NoteParent = null;
        }

        public void ClearEvents()
        {
            tempStatusChangedRepeatedly = null;
        }

        public void SetSource(NoteControlParent parent)
        {
            SetSource<object>(parent);
        }

        public void SetSource<StrictType>(NoteControlParent parent)
        {
            RestrictedType = typeof(StrictType);
            Items.Clear();
            NoteParent = parent;
            NotesContainer = null;
            Items.AddRange(InnerData.Select(x => x.NoteName).ToArray());
        }

        public void SetSource(NotesContainer parent)
        {
            SetSource<object>(parent);
        }

        public void SetSource<StrictType>(NotesContainer parent)
        {
            RestrictedType = typeof(StrictType);
            Items.Clear();
            NotesContainer = parent;
            NoteParent = null;
            Items.AddRange(InnerData.Select(x => x.NoteName).ToArray());
        }

        private void CheckText()
        {
            if (!CanBeEmpty && Text.Length == 0)
            {
                Status = InputStatus.EMPTY_FIELD;
                return;
            }
            else if (Text.Length == 0)
            {
                Status = InputStatus.UNKNOWN;
                return;
            }

            if (Text.Length < 2)
            {
                Status = InputStatus.TOO_SHORT;
                return;
            }
            if (!Items.Cast<string>().Contains(Text))
            {
                Status = InputStatus.CREATION;
                return;
            }
            Status = InputStatus.OK;
        }

        private void OnInputDetected(object? sender, EventArgs e) => CheckText();

        private void OnItemSelected(object? sender, EventArgs e)
        {
        }

        private void OnLostFocus(object? sender, EventArgs e)
        {
            CheckValid();
        }

        private void OnStatusChanged()
        {
            OnStatusChangedRepeatedly();
            if (statusChanged != null)
                statusChanged(this, Status);
        }

        private void OnStatusChangedRepeatedly()
        {
            if (statusChangedRepeatedly != null)
                statusChangedRepeatedly(this, Status);
            if (tempStatusChangedRepeatedly != null)
                tempStatusChangedRepeatedly(this, Status);
        }

        private void SetToolTip()
        {
            tooltip.SetToolTip(this, Status.GetStringValue());
        }

        private void ToggleActivity()
        {
            switch (Status)
            {
                case InputStatus.INACTIVE:
                    if (Enabled)
                        Enabled = false;
                    break;

                default:
                    if (!Enabled)
                        Enabled = true;
                    break;
            }
        }
    }
}