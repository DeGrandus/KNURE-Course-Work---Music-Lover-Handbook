using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using System.ComponentModel;
using System.Data;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls
{
    public partial class InputData : UserControl
    {
        private bool canNameBeEmpty = false;
        private bool isRenameInvalid = false;
        private InputType inputType;

        private bool IsRenameFieldTextInvalid
        {
            get => isRenameInvalid;
            set { isRenameInvalid = value; }
        }

        [Category("Data")]
        public InputType InputType
        {
            get => inputType;
            set
            {
                inputType = value;
                SetInputType(value);
            }
        }
        public SmartComboBox InputNameBox { get; }
        public TextBox InputDescriptionBox { get; }
        public bool AutoFill { get; set; } = true;

        public InputData()
        {
            InitializeComponent();
            InputDescriptionBox = descriptionBox;
            InputNameBox = boxName;
            InputNameBox.StatusChangedRepeatedly += (sender, state) =>
            {
                renameSection.Enabled = state == InputStatus.OK;
                if (!renameSection.Enabled)
                    renameCheck.Checked = false;
                InputDescriptionBox.Enabled = state != InputStatus.UNKNOWN && !state.IsError();
                UpdateRenameSection();
                if (!AutoFill)
                    return;
                if (InputDescriptionBox.Text == "" && state == InputStatus.OK)
                    InputDescriptionBox.Text =
                        InputNameBox.InnerData
                            .Find(x => x.NoteName == InputNameBox.Text)
                            ?.NoteDescription ?? "";
            };
            UpdateRenameSection();
            renameCheck.CheckedChanged += (sender, e) =>
            {
                UpdateRenameSection();
            };
            renameInput.TextChanged += (sender, e) =>
            {
                UpdateRenameField();
            };
        }

        private void UpdateRenameField()
        {
            if (!renameInput.Enabled)
            {
                renameInput.BackColor = Color.White;
                return;
            }
            IsRenameFieldTextInvalid =
                boxName.Items.Cast<string>().Contains(renameInput.Text)
                || renameInput.Text.Length < 2;
            renameInput.BackColor = Color.FromArgb(
                255,
                Color.FromArgb(
                    (int)(!IsRenameFieldTextInvalid ? InputStatus.OK : InputStatus.EMPTY_FIELD)
                )
            );
        }

        public void UpdateRenameSection()
        {
            renameInput.Enabled = renameSection.Enabled && renameCheck.Checked;
            UpdateRenameField();
        }

        public InputData(InputType mainType) : this()
        {
            InputType = mainType;
            SetLabel(InputType.ToString(true));
        }

        public void SetDataSource(NoteControlParent notes)
        {
            boxName.SetSource(notes);
        }

        public void SetDataSource<OnlyAllow>(NoteControlParent notes)
        {
            boxName.SetSource<OnlyAllow>(notes);
        }

        public void SetDataSource(NotesContainer container)
        {
            boxName.SetSource(container);
        }

        public void SetDataSource<OnlyAllow>(NotesContainer container)
        {
            boxName.SetSource<OnlyAllow>(container);
        }

        public void ClearDataSource()
        {
            boxName.ClearDataSource();
        }

        public void SetInputType(InputType type)
        {
            boxName.InputType = type;
            SetLabel(InputType.ToString(true));
        }

        public void SetLabel(string text)
        {
            noteTypeLabel.Text = text;
        }

        public void SetFont(Font font)
        {
            Font = font;
        }

        public void SetLabelFont(Font font)
        {
            noteTypeLabel.Font = font;
        }

        public OutputInfo GetOutput()
        {
            if (renameInput.Enabled && IsRenameFieldTextInvalid)
                if (renameInput.Text.Length == 0)
                    throw new InvalidDataException(
                        $"{InputType} cannot be renamed to an empty field."
                    );
                else if (renameInput.Text.Length == 1)
                    throw new InvalidDataException(
                        $"{InputType} cannot be renamed to \"{renameInput.Text}\". It is yoo short."
                    );
                else
                    throw new InvalidDataException(
                        $"{InputType} cannot be renamed to \"{renameInput.Text}\". An {InputType.ToString().ToLower()} with this name is already exists in current category."
                    );
            return new OutputInfo(
                InputType,
                InputNameBox.Status == InputStatus.OK || InputNameBox.Status == InputStatus.CREATION
                  ? InputNameBox.Text
                  : null,
                InputDescriptionBox.Text,
                renameInput.Enabled
                  ? renameInput.Text.Length > 1
                      ? renameInput.Text
                      : null
                  : null,
                Enabled
            );
        }

        public void Clean()
        {
            InputNameBox.Text = "";
            InputDescriptionBox.Text = "";
        }

        public class OutputInfo
        {
            public OutputInfo(
                InputType type,
                string? text,
                string description,
                string? replacementText,
                bool enabled
            )
            {
                Enabled = enabled;
                Type = type;
                Text = text;
                Description = description;
                ReplacementText = replacementText;
            }

            public bool Enabled { get; }
            public InputType Type { get; }
            public string? Text { get; }
            public string Description { get; }
            public string? ReplacementText { get; }

            public bool IsValid() => Enabled && Text != null;
        }
    }
}
