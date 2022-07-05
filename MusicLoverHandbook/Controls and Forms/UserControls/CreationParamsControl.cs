using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Managers;
using System.ComponentModel;
using System.Data;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls
{
    public partial class CreationParamsControl : UserControl
    {
        #region Private Fields

        private NoteType inputType;
        private bool isRenameInvalid = false;
        private BoxPathAnalyzer? pathAnalyzer;
        private string tipText = "";
        private bool useDescriptionPathAnalyzer;

        #endregion Private Fields

        #region Public Properties

        public bool AutoFill { get; set; } = true;

        public TextBox InputDescriptionBox { get; }

        public SmartComboBox InputNameBox { get; }

        [Category("Creation Data")]
        public NoteType InputType
        {
            get => inputType;
            set
            {
                inputType = value;
                SetInputType(value);
            }
        }

        [Category("Creation Data")]
        public string TipText
        {
            get => tipText;
            set
            {
                if (value == "")
                {
                    tipText = "";
                    tipLabel.Text = "";
                }
                else
                {
                    tipLabel.Text = "tip: " + value;
                    tipText = value;
                }
            }
        }

        [Category("Creation Data")]
        public bool UseDescriptionPathAnalyzer
        {
            get => useDescriptionPathAnalyzer;
            set
            {
                useDescriptionPathAnalyzer = value;
                Setup_Analyzers();
            }
        }

        #endregion Public Properties

        #region Private Properties

        private bool IsRenameFieldTextInvalid
        {
            get => isRenameInvalid;
            set { isRenameInvalid = value; }
        }

        #endregion Private Properties

        #region Public Constructors + Destructors

        public CreationParamsControl()
        {
            InitializeComponent();
            InputDescriptionBox = descriptionBox;
            InputNameBox = boxName;
            InputNameBox.StatusChangedRepeatedly += (sender, state) =>
            {
                renameSection.Enabled = state == InputStatus.OK || state == InputStatus.ANALOG;
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
            renameInput.LostFocus += (sender, e) =>
            {
                renameInput.Text = InputNameBox.Format(renameInput.Text);
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

        public CreationParamsControl(NoteType mainType) : this()
        {
            InputType = mainType;
            SetLabel(InputType.ToString(true));
        }

        #endregion Public Constructors + Destructors

        #region Public Methods

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
                        $"{InputType} cannot be renamed to \"{renameInput.Text}\". It is too short."
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

        #endregion Public Methods

        #region Protected Methods

        protected override void OnHandleDestroyed(EventArgs e)
        {
            UseDescriptionPathAnalyzer = false;
            base.OnHandleDestroyed(e);
        }

        #endregion Protected Methods

        #region Private Methods

        private void PathAnalyzerResultsChanged(PathAnalyzerResult result, string observed)
        {
            Control? control;
            while (
                (
                    control = descriptionPanel.Controls
                        .Cast<Control>()
                        .Where(c => c is not TextBox)
                        .FirstOrDefault(defaultValue: null)
                ) != null
            )
                descriptionPanel.Controls.Remove(control);
            //Debug.WriteLine("test: "+(PathAnalyzerResult.FileHasEquivalence | PathAnalyzerResult.FileInDefault));
            if (result == PathAnalyzerResult.IsNotAFile)
            {
                InputDescriptionBox.BackColor = Color.White;
                return;
            }
            else if (
                (
                    result
                    & (
                        PathAnalyzerResult.File_HasEquivalence
                        | PathAnalyzerResult.File_InMusicFolder
                        | PathAnalyzerResult.File_DoesNotExist
                        | PathAnalyzerResult.File_NotMp3
                    )
                ) > 0
            )
            {
                var notifyFileStateLabel = new Label()
                {
                    AutoSize = false,
                    Size = new(100, 32),
                    Dock = DockStyle.Bottom,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(Font.FontFamily, 18, GraphicsUnit.Pixel)
                };

                switch (result)
                {
                    case PathAnalyzerResult.File_HasEquivalence:
                        notifyFileStateLabel.Text = "File has equivalence in the default folder";
                        notifyFileStateLabel.BackColor = Color.LightYellow;
                        descriptionPanel.Controls.Add(notifyFileStateLabel);
                        break;

                    case PathAnalyzerResult.File_InMusicFolder:
                        notifyFileStateLabel.Text = "File is contained in default folder";
                        notifyFileStateLabel.BackColor = Color.LightGreen;
                        break;

                    case PathAnalyzerResult.File_DoesNotExist:
                        notifyFileStateLabel.Text = "File not exists";
                        notifyFileStateLabel.BackColor = Color.LightGray;
                        break;

                    case PathAnalyzerResult.File_NotMp3:
                        notifyFileStateLabel.Text = "File not an mp3";
                        notifyFileStateLabel.BackColor = Color.LightCoral;
                        break;
                }
                descriptionPanel.Controls.Add(notifyFileStateLabel);
            }
            else
            {
                var moveButton = new Button()
                {
                    Size = new(100, 32),
                    Dock = DockStyle.Bottom,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(Font.FontFamily, 24, GraphicsUnit.Pixel),
                    Text = "Move file to default folder",
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.LightSkyBlue,
                };
                moveButton.Click += (sender, e) =>
                {
                    var newPath = FileManager.Instance.MoveToMusicFolder(observed);
                    pathAnalyzer!.ReplacePath(newPath);
                };
                var copyButton = new Button()
                {
                    Size = new(100, 32),
                    Dock = DockStyle.Bottom,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(Font.FontFamily, 24, GraphicsUnit.Pixel),
                    Text = "Copy file to default folder",
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.LightSkyBlue,
                };
                copyButton.Click += (sender, e) =>
                {
                    var newPath = FileManager.Instance.CopyToMusicFolder(observed);
                    pathAnalyzer!.ReplacePath(newPath);
                };
                descriptionPanel.Controls.Add(moveButton);
                descriptionPanel.Controls.Add(copyButton);
            }
        }

        private void Setup_Analyzers()
        {
            if (pathAnalyzer != null)
            {
                pathAnalyzer.Dispose();
                pathAnalyzer = null;
            }

            if (UseDescriptionPathAnalyzer)
            {
                pathAnalyzer = new BoxPathAnalyzer(InputDescriptionBox);
                pathAnalyzer.ResultsChanged += PathAnalyzerResultsChanged;
            }
        }

        private void UpdateRenameField()
        {
            if (!renameInput.Enabled)
            {
                renameInput.BackColor = Color.White;
                renameInput.Text = "";
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

        #endregion Private Methods

        #region Public Classes + Structs

        public class OutputInfo
        {
            #region Public Properties

            public string Description { get; }

            public bool Enabled { get; }

            public string? ReplacementText { get; }

            public string? Text { get; }

            public NoteType Type { get; }

            #endregion Public Properties

            #region Public Constructors + Destructors

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

            #endregion Public Constructors + Destructors

            #region Public Methods

            public bool IsValid() => Enabled && Text != null;

            #endregion Public Methods
        }

        #endregion Public Classes + Structs
    }
}