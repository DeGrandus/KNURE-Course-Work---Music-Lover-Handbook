using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Controls_and_Forms.UserControls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using System.Diagnostics;
using TagFile = TagLib.File;

namespace MusicLoverHandbook.View.Forms
{
    public partial class AddNoteMenu : Form
    {
        public MainForm MainForm { get; }
        public AddNoteMenu(MainForm mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
            SetupLayout();
        }
        private void SetupLayout()
        {

            StartPosition = FormStartPosition.Manual;
            var fontfam = FontContainer.Instance.Families[0];
            Font = new Font(fontfam,12,GraphicsUnit.Point);
            Size = new Size(750,MainForm.Height-20);
            MinimumSize = new Size(400, 750);
            Location = new Point(MainForm.Location.X+(MainForm.Width - Width)/2, MainForm.Location.Y+(MainForm.Height - Height)/2);

            title.BackColor = MainForm.title.BackColor;
            title.Size = MainForm.title.Size;
            title.Font = MainForm.title.Font;

            createButton.Size = new Size(0, 70);
            createButton.Font = new Font(Font.FontFamily, 50, GraphicsUnit.Pixel);
            createButton.BackColor = MainForm.title.BackColor;
            createButton.FlatAppearance.BorderColor = ControlPaint.DarkDark(createButton.BackColor);
            createButton.FlatAppearance.BorderSize = 4;

            dragDropPanel.BackColor = MainForm.title.BackColor;
            dragDropText.BackColor = ControlPaint.LightLight(MainForm.title.BackColor);
            dragDropText.Font = createButton.Font;
            dragDropText.AllowDrop = true;
            dragDropText.DragEnter += (sender, e) =>
            {
                if (e.Data == null) return;
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
                Debug.WriteLine(String.Join(',', ((string[])e.Data.GetData(DataFormats.FileDrop))));
                if (((string[])e.Data.GetData(DataFormats.FileDrop)).Where(x => x.Contains(".mp3")).Count() > 0)
                {
                    e.Effect = DragDropEffects.Link;
                }
                else
                    e.Effect = DragDropEffects.None;
            };
            dragDropText.DragDrop += (sender, e) =>
            {
                if (e.Data == null) return;
                if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
                if (e.Effect != DragDropEffects.Link) return;

                var mp3 = ((string[])e.Data.GetData(DataFormats.FileDrop)).Where(x => x.Contains(".mp3")).ToArray()[0];
                ApplyFileData(mp3);
                    
            };

            inputAuthor.SetInputType(InputType.Author);
            inputAuthor.SetDataSource(MainForm.Container);

            inputAuthor.InputNameBox.StateChanged += (sender, state) =>
            {
                if (state == InputState.OK)
                {
                    var data = (sender.InnerData.Find(x => x.NoteText == sender.Text) as NoteControlParent);
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
                var dataSelf = (sender.InnerData.Find(x => x.NoteText == sender.Text) as NoteControlMidder);

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
                    var dataSongFiles = (sender.InnerData.Find(x => x.NoteText == sender.Text) as NoteControlParent);
                    Debug.WriteLine($"Event state data is : \n{dataSongFiles}");
                    if (dataSongFiles != null)
                    {
                        inputSongFile.SetDataSource<NoteSongFile>(dataSongFiles);
                    }
                }
                else
                    inputSongFile.ClearDataSource();

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
            if (!File.Exists(filePath)) return;

            var file = TagFile.Create(filePath);
            var tag = file.Tag;
            
            string name = tag.Title??"";
            string filename = Path.GetFileName(file.Name)??"";
            string authors = string.Join(" & ", tag.Performers);
            string album = tag.Album;

            string comment = tag.Comment;
            uint year = tag.Year;
            string genres = string.Join(", ", tag.Genres);

            var description = "";
            description += year!=0?$"Year: {year}\r\n":"";
            description += genres!="" ? $"Genre: {genres}\r\n" : "";
            description += comment != "" ? $"Comment: {comment}\r\n" : "";

            inputAuthor.InputNameBox.Text = authors;
            inputDisc.InputNameBox.Text = album;
            inputSong.InputNameBox.Text = name;
            inputSongFile.InputNameBox.Text = filename;
            inputSongFile.InputDescriptionBox.Text = description;

        }


    }
}
