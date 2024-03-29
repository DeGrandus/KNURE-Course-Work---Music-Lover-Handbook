﻿using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Delegates;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Inerfaces;
using System.Text.RegularExpressions;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class SmartComboBox : ComboBox, ISmartComboBox
    {
        #region Public Fields

        public bool CanBeEmpty = false;

        public string? DefaultReplacement;

        public bool SkipNextErrorToolTip = false;

        #endregion Public Fields

        #region Private Fields

        private InputStatus status;

        private StateChangedEvent? statusChanged;

        private StateChangedEvent? statusChangedRepeatedly;

        private StateChangedEvent? tempStatusChangedRepeatedly;

        private ToolTip tooltip = new ToolTip() { InitialDelay = 0 };

        #endregion Private Fields

        #region Public Properties

        public List<NoteControl> InnerData =>
            (
                from note in (
                    NoteParent?.InnerNotes.OfType<NoteControl>().ToList()
                    ?? NotesContainer?.InnerNotes.OfType<NoteControl>().ToList()
                    ?? new()
                )
                where
                    note.GetType().IsSubclassOf(RestrictedType)
                    || note.GetType().Equals(RestrictedType)
                select note
            ).ToList();

        public NoteType InputType { get; set; }

        public NoteControlParent? NoteParent { get; set; }

        public NotesContainer? NotesContainer { get; set; }

        public Type RestrictedType { get; set; } = typeof(object);

        public InputStatus Status
        {
            get => status;
            set
            {
                if (status == value)
                {
                    OnStatusChangedRepeatedly();
                    return;
                }
                BackColor = Color.FromArgb(255, Color.FromArgb((int)value));
                tooltip.BackColor = ControlPaint.Light(BackColor, 0.8f);
                status = value;
                //Debug.WriteLine($"Change State to {value} is {this}");

                OnStatusChanged();
                ToggleActivity();
                SetToolTip();
            }
        }

        #endregion Public Properties

        #region Public Constructors + Destructors

        public SmartComboBox()
        {
            tooltip.OwnerDraw = true;
            tooltip.Draw += (sender, e) =>
            {
                e.DrawBackground();
                e.DrawBorder();
                e.DrawText();
            };
            TextChanged += OnInputDetected;
            LostFocus += OnLostFocus;
        }

        #endregion Public Constructors + Destructors

        #region Public Methods

        public void CheckTextValidation()
        {
            CheckForStatus();
            if (Text != Format(Text))
                Text = Format(Text);
            if ((!CanBeEmpty && Status == InputStatus.EMPTY_FIELD) || Status.IsError())
            {
                Text = DefaultReplacement ?? $"Unknown {InputType.ToString() ?? "???"}";
                if (SkipNextErrorToolTip == true && !(SkipNextErrorToolTip = false))
                    return;

                new ToolTip()
                {
                    IsBalloon = true,
                    ToolTipIcon = ToolTipIcon.Error,
                    ToolTipTitle = "Naming error"
                }.Show("Name is not valid!", this, Width - 20, -70 - Height / 2, 2000);
            }
        }

        public void ClearDataSource()
        {
            RestrictedType = typeof(object);
            Items.Clear();
            NotesContainer = null;
            NoteParent = null;
        }

        public void ClearTempEvents()
        {
            tempStatusChangedRepeatedly = null;
        }

        public string Format(string toFormat)
        {
            return Regex.Replace(toFormat, @"([ ;^*@%!+-/|\.,><'""$#№(){}\[\]\\])\1+", "$1").Trim();
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

        #endregion Public Methods

        #region Protected Methods

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }

        #endregion Protected Methods

        #region Private Methods

        private void CheckForStatus()
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
            if (HasAnalog() is int analogInd)
            {
                Status = InputStatus.ANALOG;
                Tag = InnerData[analogInd].NoteName;
                return;
            }
            if (!Items.Cast<string>().Contains(Text))
            {
                Status = InputStatus.CREATION;
                return;
            }

            Tag = Text;
            Status = InputStatus.OK;
        }

        private int? HasAnalog()
        {
            var inner = InnerData.Select(n => n.NoteName).ToList();
            var cont = inner.Find(
                x =>
                {
                    return x.ToLower().Trim() == Text.ToLower().Trim() && x != Text;
                }
            );

            return cont != null ? inner.IndexOf(cont) : null;
        }

        private void OnInputDetected(object? sender, EventArgs e) => CheckForStatus();

        private void OnItemSelected(object? sender, EventArgs e)
        {
        }

        private void OnLostFocus(object? sender, EventArgs e)
        {
            CheckTextValidation();
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

        #endregion Private Methods

        #region Public Events + Delegates

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

        #endregion Public Events + Delegates
    }
}