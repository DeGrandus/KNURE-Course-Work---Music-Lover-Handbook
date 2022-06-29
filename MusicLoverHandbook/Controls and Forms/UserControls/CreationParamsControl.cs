using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static MusicLoverHandbook.Controls_and_Forms.UserControls.CreationParamsControl.BoxPathAnalyzer;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls
{
    public partial class CreationParamsControl : UserControl
    {
        private bool canNameBeEmpty = false;
        private NoteType inputType;
        private bool isRenameInvalid = false;

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
        private string tipText = "";

        private bool useDescriptionPathAnalyzer;

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

        public class BoxPathAnalyzer : IDisposable
        {
            private TextBox box;

            private void OnBoxTextChanged(object? sender, EventArgs e) => AnalyzeBoxText();

            public void AnalyzeBoxText()
            {
                var obeservedString = Regex
                    .Replace(box.Text, @"(\n|\r)+", "\r\n")
                    .Split("\r\n")
                    .FirstOrDefault("");
                OnResultsChange(StringAnalyzer(obeservedString), obeservedString);
            }

            private FileSystemWatcher? defaultWatcher;

            private PathAnalyzerResult StringAnalyzer(string observedString)
            {
                if (FileManager.Instance.CheckMusicFilePathOrName(observedString))
                {
                    ReplacePath(Path.GetFileName(observedString));
                    return PathAnalyzerResult.FileInDefault;
                }
                if (!Path.IsPathRooted(observedString))
                    return PathAnalyzerResult.NotAFile;
                if (!File.Exists(observedString))
                    return PathAnalyzerResult.FileNotExist;
                if (Path.GetExtension(observedString).ToLower() != ".mp3")
                    return PathAnalyzerResult.FileNotMp3;

                observedFileWatcher!.Path = Path.GetFullPath(
                    Path.GetDirectoryName(observedString)!
                );
                observedFileWatcher.Filter = Path.GetFileName(observedString);
                observedFileWatcher.EnableRaisingEvents = true;
                if (
                    Path.GetFullPath(Path.GetDirectoryName(observedString) ?? "???")
                        .TrimEnd('\\', '/')
                        .ToLower()
                    != Path.GetFullPath(FileManager.Instance.MusicFilesFolderPath)
                        .TrimEnd('\\', '/')
                        .ToLower()
                )
                    if (
                        Directory
                            .GetFiles(FileManager.Instance.MusicFilesFolderPath + '\\', "*.mp3")
                            .Any(f => Path.GetFileName(f) == Path.GetFileName(observedString))
                    )
                        return PathAnalyzerResult.FileHasEquivalence;
                    else
                        return PathAnalyzerResult.FileNotInDefault;

                return PathAnalyzerResult.FileInDefault;
            }

            private FileSystemWatcher? observedFileWatcher;

            public void Dispose()
            {
                box.TextChanged -= OnBoxTextChanged;
                resultsChanged = null;
                if (defaultWatcher == null && observedFileWatcher == null)
                    return;

                Dispose_Watcher(defaultWatcher);
                Dispose_Watcher(observedFileWatcher);

                defaultWatcher = null;
                observedFileWatcher = null;
            }

            private void Setup_WatcherEvents(FileSystemWatcher watcher)
            {
                watcher.Created += OnDefaultMusicFolderContentChanged;
                watcher.Changed += OnDefaultMusicFolderContentChanged;
                watcher.Renamed += OnDefaultMusicFolderContentChanged;
                watcher.Deleted += OnDefaultMusicFolderContentChanged;
            }

            private void Dispose_Watcher(FileSystemWatcher? watcher)
            {
                if (watcher == null)
                    return;
                watcher.EnableRaisingEvents = false;
                watcher!.Changed -= OnDefaultMusicFolderContentChanged;
                watcher!.Renamed -= OnDefaultMusicFolderContentChanged;
                watcher!.Deleted -= OnDefaultMusicFolderContentChanged;
                watcher!.Created -= OnDefaultMusicFolderContentChanged;
                watcher.Dispose();
            }

            public void ReplacePath(string newPath)
            {
                box.Text = string.Join(
                    "\r\n",
                    Regex
                        .Replace(box.Text, @"(\n|\r)+", "\r\n")
                        .Split("\r\n")
                        .Skip(1)
                        .Reverse()
                        .Concat(new[] { newPath })
                        .Reverse()
                );
            }

            private void OnDefaultMusicFolderContentChanged(object sender, FileSystemEventArgs e) =>
                box.Invoke(() => AnalyzeBoxText());

            public BoxPathAnalyzer(TextBox analyzeBox)
            {
                box = analyzeBox;
                box.TextChanged += OnBoxTextChanged;
                defaultWatcher = new(
                    Path.GetFullPath(FileManager.Instance.MusicFilesFolderPath),
                    "*.mp3"
                )
                {
                    EnableRaisingEvents = true
                };
                observedFileWatcher = new FileSystemWatcher()
                {
                    EnableRaisingEvents = false,
                    Path = ""
                };
                Setup_WatcherEvents(defaultWatcher);
                Setup_WatcherEvents(observedFileWatcher);
            }

            [Flags]
            public enum PathAnalyzerResult
            {
                FileNotMp3 = 1,
                FileHasEquivalence = 2,
                FileNotExist = 4,
                FileInDefault = 8,
                FileNotInDefault = 16,
                NotAFile = 32,
            }

            private void OnResultsChange(
                PathAnalyzerResult pathAnalyzerResult,
                string obeservedString
            )
            {
                if (resultsChanged != null)
                    resultsChanged(pathAnalyzerResult, obeservedString);
                Debug.WriteLine(pathAnalyzerResult);
            }

            public delegate void PathAnalyzerResultHandler(
                PathAnalyzerResult result,
                string obeservedString
            );
            private PathAnalyzerResultHandler? resultsChanged;
            public event PathAnalyzerResultHandler ResultsChanged
            {
                add => resultsChanged += value;
                remove => resultsChanged -= value;
            }
        }

        private BoxPathAnalyzer? pathAnalyzer;

        protected override void OnHandleDestroyed(EventArgs e)
        {
            UseDescriptionPathAnalyzer = false;
            base.OnHandleDestroyed(e);
        }
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
            if (result == PathAnalyzerResult.NotAFile)
            {
                InputDescriptionBox.BackColor = Color.White;
                return;
            }
            else if (
                (
                    result
                    & (
                        PathAnalyzerResult.FileHasEquivalence
                        | PathAnalyzerResult.FileInDefault
                        | PathAnalyzerResult.FileNotExist
                        | PathAnalyzerResult.FileNotMp3
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
                    case PathAnalyzerResult.FileHasEquivalence:
                        notifyFileStateLabel.Text = "File has equivalence in the default folder";
                        notifyFileStateLabel.BackColor = Color.LightYellow;
                        descriptionPanel.Controls.Add(notifyFileStateLabel);
                        break;
                    case PathAnalyzerResult.FileInDefault:
                        notifyFileStateLabel.Text = "File is contained in default folder";
                        notifyFileStateLabel.BackColor = Color.LightGreen;
                        break;
                    case PathAnalyzerResult.FileNotExist:
                        notifyFileStateLabel.Text = "File not exists";
                        notifyFileStateLabel.BackColor = Color.LightGray;
                        break;
                    case PathAnalyzerResult.FileNotMp3:
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
        private bool IsRenameFieldTextInvalid
        {
            get => isRenameInvalid;
            set { isRenameInvalid = value; }
        }

        public CreationParamsControl()
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

        public CreationParamsControl(NoteType mainType) : this()
        {
            InputType = mainType;
            SetLabel(InputType.ToString(true));
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
            public string Description { get; }

            public bool Enabled { get; }

            public string? ReplacementText { get; }

            public string? Text { get; }

            public NoteType Type { get; }

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

            public bool IsValid() => Enabled && Text != null;
        }
    }
}
