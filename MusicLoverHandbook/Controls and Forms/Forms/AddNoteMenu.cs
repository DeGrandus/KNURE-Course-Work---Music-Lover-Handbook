using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Controls_and_Forms.UserControls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Diagnostics;
using static MusicLoverHandbook.Controls_and_Forms.UserControls.InputData;
using TagFile = TagLib.File;

namespace MusicLoverHandbook.View.Forms
{
    public partial class AddNoteMenu : Form
    {
        public MainForm MainForm { get; }
        public NoteCreationType CreationType { get; private set; } = NoteCreationType.DiscInAuthor;
        public NoteControlParent? FinalNote { get; private set; }
        private List<InputData> allInputs;

        public AddNoteMenu(MainForm mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
            SetupLayout();
        }

        public void SwitchType()
        {
            CreationType =
                CreationType == NoteCreationType.DiscInAuthor
                    ? NoteCreationType.AuthorInDisc
                    : NoteCreationType.DiscInAuthor;
        }

        private void SetupLayout()
        {
            allInputs = tableInputs.Controls
                .Cast<Control>()
                .Where(x => x is InputData)
                .Cast<InputData>()
                .ToList();
            StartPosition = FormStartPosition.Manual;
            var fontfam = FontContainer.Instance.Families[0];
            Font = new Font(fontfam, 12, GraphicsUnit.Point);
            Size = new Size(750, MainForm.Height - 20);
            MinimumSize = new Size(400, 750);
            Location = new Point(
                MainForm.Location.X + (MainForm.Width - Width) / 2,
                MainForm.Location.Y + (MainForm.Height - Height) / 2
            );

            title.BackColor = MainForm.title.BackColor;
            title.Size = MainForm.title.Size;
            title.Font = MainForm.title.Font;

            createButton.Size = new Size(0, 70);
            createButton.Font = new Font(Font.FontFamily, 50, GraphicsUnit.Pixel);
            createButton.BackColor = MainForm.title.BackColor;
            createButton.FlatAppearance.BorderColor = ControlPaint.DarkDark(createButton.BackColor);
            createButton.FlatAppearance.BorderSize = 4;
            createButton.Click += (sender, e) =>
            {
                NoteCreator creator;
                try
                {
                    creator = new NoteCreator(
                        MainForm,
                        CreationType,
                        allInputs.Select(x => x.GetOutput()).ToArray()
                    );
                }
                catch (InvalidDataException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Renaming error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }
                FinalNote = creator.CreateNote();
                DialogResult = DialogResult.OK;
                Close();
            };

            dragDropPanel.BackColor = MainForm.title.BackColor;
            dragDropText.BackColor = ControlPaint.LightLight(MainForm.title.BackColor);
            dragDropText.Font = createButton.Font;
            dragDropText.AllowDrop = true;
            dragDropText.DragEnter += (sender, e) =>
            {
                if (e.Data == null)
                    return;
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;
                Debug.WriteLine(String.Join(',', ((string[])e.Data.GetData(DataFormats.FileDrop))));
                if (
                    ((string[])e.Data.GetData(DataFormats.FileDrop))
                        .Where(x => x.Contains(".mp3"))
                        .Count() > 0
                )
                {
                    e.Effect = DragDropEffects.Link;
                }
                else
                    e.Effect = DragDropEffects.None;
            };
            dragDropText.DragDrop += (sender, e) =>
            {
                if (e.Data == null)
                    return;
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;
                if (e.Effect != DragDropEffects.Link)
                    return;

                var mp3 = ((string[])e.Data.GetData(DataFormats.FileDrop))
                    .Where(x => x.Contains(".mp3"))
                    .ToArray()[0];
                ApplyFileData(mp3);
            };

            inputAuthor.SetInputType(InputType.Author);
            inputAuthor.SetDataSource<NoteAuthor>(MainForm.Container);

            inputAuthor.InputNameBox.StateChanged += (sender, state) =>
            {
                if (state == InputState.OK)
                {
                    var data = (
                        sender.InnerData.Find(x => x.NoteText == sender.Text) as NoteControlParent
                    );
                    Debug.WriteLine($"Event state data is : \n{data}");
                    if (data != null)
                    {
                        inputDisc.SetDataSource<NoteDisc>(data);
                        inputSong.SetDataSource<NoteSong>(data);
                    }
                }
                else
                {
                    inputDisc.ClearDataSource();
                    inputSong.ClearDataSource();
                }

                inputDisc.InputNameBox.CheckValid();
                inputSong.InputNameBox.CheckValid();
            };

            inputDisc.SetInputType(InputType.Disc);
            inputDisc.InputNameBox.CanBeEmpty = true;
            inputDisc.InputNameBox.StateChanged += (sender, state) =>
            {
                var dataSelf = (
                    sender.InnerData.Find(x => x.NoteText == sender.Text) as NoteControlMidder
                );

                if (state == InputState.OK)
                {
                    var dataSongs = dataSelf?.InnerNotes;
                    Debug.WriteLine($"Event state data is : \n{dataSongs}");
                    if (dataSelf != null)
                    {
                        inputSong.SetDataSource<NoteSong>(dataSelf);
                    }
                }
                else
                {
                    if (sender.NoteParent != null)
                        inputSong.SetDataSource<NoteSong>(sender.NoteParent);
                }
                inputSong.InputNameBox.CheckValid();
            };

            inputSong.SetInputType(InputType.SongName);
            inputSong.InputNameBox.CanBeEmpty = true;
            inputSong.InputNameBox.StateChanged += (sender, state) =>
            {
                if (state == InputState.OK)
                {
                    var dataSongFiles = (
                        sender.InnerData.Find(x => x.NoteText == sender.Text) as NoteControlParent
                    );
                    Debug.WriteLine($"Event state data is : \n{dataSongFiles}");
                    if (dataSongFiles != null)
                    {
                        inputSongFile.SetDataSource<NoteSongFile>(dataSongFiles);
                    }
                }
                else
                {
                    inputSongFile.ClearDataSource();
                }
                if (state == InputState.UNKNOWN)
                {
                    inputSongFile.Enabled = false;
                    inputSongFile.Clean();
                }
                else
                    inputSongFile.Enabled = true;

                inputSongFile.InputNameBox.CheckValid();
            };

            inputSongFile.SetInputType(InputType.SongFile);
            inputSongFile.InputNameBox.CanBeEmpty = true;

            var inputs = tableInputs.Controls.Cast<Control>();
            foreach (var input in inputs)
                if (input is InputData inputData)
                {
                    inputData.SetLabelFont(new Font(Font.FontFamily, 18, GraphicsUnit.Point));
                    inputData.InputNameBox.CheckValid();
                }
        }

        public void ApplyFileData(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            var file = TagFile.Create(filePath);
            var tag = file.Tag;

            string name = tag.Title ?? "";
            string filename = Path.GetFileName(file.Name) ?? "";
            string authors = string.Join(" & ", tag.Performers);
            string album = tag.Album;

            string comment = tag.Comment;
            uint year = tag.Year;
            string genres = string.Join(", ", tag.Genres);

            var descSong = "";
            descSong += year != 0 ? $"Year: {year}\r\n" : "";
            descSong += genres != "" ? $"Genre: {genres}\r\n" : "";
            descSong += comment != "" ? $"Comment: {comment}\r\n" : "";

            var descFile = "";
            descFile += $"{filePath}\r\n";
            descFile +=
                $"Duration: {string.Format("{0:hh\\:mm\\:ss}", file.Properties.Duration)}\r\n";
            descFile += $"Bitrate: {file.Properties.AudioBitrate}\r\n";

            allInputs.ForEach(x => x.AutoFill = false);
            if (
                allInputs
                    .Select(x => x.InputNameBox.Status)
                    .Where(x => x == InputState.OK || x == InputState.CREATION)
                    .Count() > 1
            )
            {
                var result = MessageBox.Show(
                    "There are some fields that are already filled with some data. Replace with file data (YES) or fill up only remain data (NO)",
                    "Information load question",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (result != DialogResult.Cancel)
                {
                    if (result == DialogResult.Yes)
                    {
                        inputAuthor.InputNameBox.Text = authors;
                        inputDisc.InputNameBox.Text = album;
                        inputSong.InputNameBox.Text = name;
                        inputSong.InputDescriptionBox.Text = descSong;
                    }
                    if (
                        inputSong.InputNameBox.Status is InputState x
                        && !(x == InputState.OK || x == InputState.CREATION)
                    )
                        inputSong.InputNameBox.Text = name;
                    inputSongFile.InputNameBox.Text = filename;
                    inputSongFile.InputDescriptionBox.Text = descFile;
                }
            }
            else
            {
                inputAuthor.InputNameBox.Text = authors;
                inputDisc.InputNameBox.Text = album;
                inputSong.InputNameBox.Text = name;
                inputSong.InputDescriptionBox.Text = descSong;
                inputSongFile.InputNameBox.Text = filename;
                inputSongFile.InputDescriptionBox.Text = descFile;
            }

            allInputs.ForEach(
                x =>
                {
                    x.InputNameBox.CheckValid();
                }
            );
        }

        public class NoteCreator
        {
            public NoteCreationType Type { get; }
            public Dictionary<InputType, OutputInfo> Info { get; }
            public MainForm Form { get; }

            public NoteCreator(MainForm form, NoteCreationType creationType, OutputInfo[] info)
            {
                Type = creationType;
                Info = info.Select(x => (Type: x.Type, Data: x))
                    .ToDictionary(x => x.Type, x => x.Data);
                Form = form;
            }

            public NoteControlParent CreateNote()
            {
                var contaierData = Form.Container.Hierarchy;

                var starterType = InputType.Author;
                var nextType = InputType.Disc;
                if (Type == NoteCreationType.AuthorInDisc)
                    (starterType, nextType) = (nextType, starterType);

                var starterInfo = Info[starterType];
                var nextInfo = Info[nextType];
                var songInfo = Info[InputType.SongName];
                var songFileInfo = Info[InputType.SongFile];

                var hierStarter =
                    contaierData.Find(x => x.NoteText == starterInfo.Text) as NoteControlParent
                    ?? null;
                if (hierStarter == null)
                    hierStarter =
                        starterType == InputType.Author
                            ? new NoteAuthor(null, starterInfo.Text, starterInfo.Description)
                            : new NoteDisc(null, starterInfo.Text, starterInfo.Description);
                else if (starterInfo.ReplacementText != null)
                    hierStarter.NoteText = starterInfo.ReplacementText;
                hierStarter.NoteDescription = starterInfo.Description;

                if (!nextInfo.IsValid())
                    return hierStarter;

                var nextElement =
                    hierStarter.InnerNotes.ToList().Find(x => x.NoteText == nextInfo.Text)
                        as NoteControlParent
                    ?? null;
                if (nextElement == null)
                {
                    nextElement =
                        nextType == InputType.Author
                            ? new NoteAuthor(
                                  (NoteDisc)hierStarter,
                                  nextInfo.Text,
                                  nextInfo.Description
                              )
                            : new NoteDisc(
                                  (NoteAuthor)hierStarter,
                                  nextInfo.Text,
                                  nextInfo.Description
                              );
                    hierStarter.InnerNotes.Add((INoteControlChild)nextElement);
                }
                else if (nextInfo.ReplacementText != null)
                    nextElement.NoteText = nextInfo.ReplacementText;
                nextElement.NoteDescription = nextInfo.Description;

                if (!songInfo.IsValid())
                    return hierStarter;

                var song =
                    nextElement.InnerNotes.ToList().Find(x => x.NoteText == songInfo.Text)
                        as NoteControlParent
                    ?? null;
                if (song == null)
                {
                    song = new NoteSong(nextElement, songInfo.Text, songInfo.Description);
                    nextElement.InnerNotes.Add((INoteControlChild)song);
                }
                else if (songInfo.ReplacementText != null)

                    song.NoteText = songInfo.ReplacementText;

                song.NoteDescription = songInfo.Description;

                if (!songFileInfo.IsValid())
                    return hierStarter;

                var songFile =
                    song.InnerNotes.ToList().Find(x => x.NoteText == songFileInfo.Text)
                        as NoteControlChild
                    ?? null;
                if (songFile == null)
                {
                    songFile = new NoteSongFile(
                        (NoteSong)song,
                        songFileInfo.Text,
                        songFileInfo.Description
                    );
                    song.InnerNotes.Add(songFile);
                }
                else if (songFileInfo.ReplacementText != null)
                {
                    songFile.NoteText = songFileInfo.ReplacementText;
                }
                songFile.NoteDescription = songFileInfo.Description;

                return hierStarter;
            }
        }
    }
}
