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
        private NoteType inputType;
        private bool isRenameInvalid = false;

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

        public InputData(NoteType mainType) : this()
        {
            InputType = mainType;
            SetLabel(InputType.ToString(true));
        }

        public bool AutoFill { get; set; } = true;

        public TextBox InputDescriptionBox { get; }

        public SmartComboBox InputNameBox { get; }

        [Category("Data")]
        public NoteType InputType
        {
            get => inputType;
            set
            {
                inputType = value;
                SetInputType(value);
            }
        }

        private bool IsRenameFieldTextInvalid
        {
            get => isRenameInvalid;
            set { isRenameInvalid = value; }
        }

        public void Clean()
        {
            InputNameBox.Text = "";
            InputDescriptionBox.Text = "";
        }

        public void ClearDataSource()
        {
            boxName.ClearDataSource();
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
            if (InputNameBox.Status == InputStatus.ANALOG)
                if (
                    MessageBox.Show(
                        $@"Field with name ""{InputNameBox.Text}"" has an already existing analog ""{(string)InputNameBox.Tag}"". If you want to update an already created note, select OK. Otherwise - Cancel.",
                        $@"Field ""{InputType}"" has an analog",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                    ) == DialogResult.OK
                )
                    InputNameBox.Text = (string)InputNameBox.Tag;
                else
                    throw new OperationCanceledException("Operation canceled");
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

        public void SetFont(Font font)
        {
            Font = font;
        }

        public void SetInputType(NoteType type)
        {
            boxName.InputType = type;
            SetLabel(InputType.ToString(true));
        }

        public void SetLabel(string text)
        {
            noteTypeLabel.Text = text;
        }

        public void SetLabelFont(Font font)
        {
            noteTypeLabel.Font = font;
        }

        public void UpdateRenameSection()
        {
            renameInput.Enabled = renameSection.Enabled && renameCheck.Checked;
            UpdateRenameField();
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

        public class OutputInfo
        {
            public OutputInfo(
                NoteType type,
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

            public string Description { get; }
            public bool Enabled { get; }
            public string? ReplacementText { get; }
            public string? Text { get; }
            public NoteType Type { get; }

            public bool IsValid() => Enabled && Text != null;
        }
    }
}
