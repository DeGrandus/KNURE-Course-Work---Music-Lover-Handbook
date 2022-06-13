using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Timer = System.Windows.Forms.Timer;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class SmartComboBox : ComboBox, ISmartComboBox
    {
        public SmartComboBox()
        {
            TextChanged += OnInputDetected;
            LostFocus += OnLostFocus;
        }

        public string? DefaultReplacement;
        public bool CanBeEmpty = false;
        public InputType InputType { get; set; }
        public NoteControlParent? NoteParent { get; set; }
        public NotesContainer? NotesContainer { get; set; }
        public Type RestrictedType { get; set; } = typeof(object);
        public List<NoteControl> InnerData =>
            (
                NoteParent?.InnerNotes.Where(x => x is NoteControl).Cast<NoteControl>().ToList()
                ?? NotesContainer
                    ?.Hierarchy.Where(x => x is NoteControl)
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

        private void OnLostFocus(object? sender, EventArgs e)
        {
            CheckValid();
        }



        public void CheckValid()
        {
            CheckText();
            if (!CanBeEmpty && Status.IsError())
                Text = DefaultReplacement ?? $"Unknown {InputType.ToString() ?? "???"}";
        }

        private InputState state;
        private ToolTip tooltip = new ToolTip() { InitialDelay = 0, IsBalloon = true, };
        public InputState Status
        {
            get => state;
            set
            {
                if (state == value) return;
                BackColor = Color.FromArgb(255, Color.FromArgb((int)value));
                state = value;
                Debug.WriteLine($"Change State to {value} is {this}");
                OnStateChanged();
                ToggleActivity();
                SetToolTip();
            }
        }

        private void SetToolTip()
        {
            tooltip.SetToolTip(this, Status.GetStringValue());
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
            Items.AddRange(InnerData.Select(x => x.NoteText).ToArray());
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
            Items.AddRange(InnerData.Select(x => x.NoteText).ToArray());
        }

        public void ClearDataSource()
        {
            RestrictedType = typeof(object);
            Items.Clear();
            NotesContainer = null;
            NoteParent = null;
        }

        private void OnItemSelected(object? sender, EventArgs e) { }

        private void OnInputDetected(object? sender, EventArgs e)
        {
            CheckText();
        }

        private void CheckText()
        {
            if (!CanBeEmpty && Text.Length == 0)
            {
                Status = InputState.EMPTY_FIELD;
                return;
            }
            else if (Text.Length == 0)
            {
                Status = InputState.UNKNOWN;
                return;
            }

            if (Text.Length < 2)
            {
                Status = InputState.TOO_SHORT;
                return;
            }
            if (!Items.Cast<string>().Contains(Text))
            {
                Status = InputState.CREATION;
                return;
            }
            Status = InputState.OK;
        }

        private void ToggleActivity()
        {
            switch (Status)
            {
                case InputState.INACTIVE:
                    if (Enabled)
                        Enabled = false;
                    break;
                default:
                    if (!Enabled)
                        Enabled = true;
                    break;
            }
        }

        private void OnStateChanged()
        {
            if (StateChanged != null)
                StateChanged(this, Status);
        }

        public delegate void StateChangedEvent(SmartComboBox sender, InputState state);
        public event StateChangedEvent StateChanged;
    }
}
