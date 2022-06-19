using MusicLoverHandbook.Controller;
using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Controls_and_Forms.UserControls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Logic.Notes;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using System.Diagnostics;
using static MusicLoverHandbook.Controls_and_Forms.Custom_Controls.SmartComboBox;
using static MusicLoverHandbook.Controls_and_Forms.UserControls.InputData;
using static System.Windows.Forms.AxHost;
using TagFile = TagLib.File;

namespace MusicLoverHandbook.View.Forms
{
    public partial class AddNoteMenu : Form
    {
        public MainForm MainForm { get; }
        public NoteCreationType CreationType
        {
            get => creationType; set
            {
                if (creationType == value) return;

                if (value is NoteCreationType.DiscInAuthor)
                    SelectedCreationTypeLabel = discInAuthorLabel;
                else
                    SelectedCreationTypeLabel = authorInDiscLabel;

                creationType = value;
                SetupInputs();
            }
        }
        public NoteControlMidder? FinalNote { get; private set; }
        public LinkedList<InputData> InputDataOrdered = new();
        public LinkedList<Action<SmartComboBox, InputState>> InputEventsOrdered = new();
        private List<InputData> allInputs => InputDataOrdered.ToList();
        private Label SelectedCreationTypeLabel
        {
            get => selectedCreationTypeLabel; set
            {
                if (selectedCreationTypeLabel != value)
                    (selectedCreationTypeLabel.BackColor, value.BackColor) = (value.BackColor, selectedCreationTypeLabel.BackColor);
                selectedCreationTypeLabel = value;
            }
        }

        public AddNoteMenu(MainForm mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
            InputDataOrdered = new LinkedList<InputData>();
            SetupSwitchButtons();
            SetupLayout();
        }
        private void SetupInputsOrder()
        {
            InputDataOrdered.Clear();
            InputData main = InputAuthor, secondary = InputDisc;
            if (CreationType == NoteCreationType.AuthorInDisc)
                (main, secondary) = (secondary, main);
            InputDataOrdered.AddLast(main);
            InputDataOrdered.AddLast(secondary);
            InputDataOrdered.AddLast(InputSong);
            InputDataOrdered.AddLast(InputSongFile);

            for (var inp = InputDataOrdered.First; inp != null; inp = inp.Next)
                tableInputs.Controls.Add(inp.Value, 0, allInputs.IndexOf(inp.Value));

        }

        private Action<object?, EventArgs> swapTypesAction;
        private NoteCreationType creationType = NoteCreationType.DiscInAuthor;
        private Label selectedCreationTypeLabel;

        private void SetupSwitchButtons()
        {
            selectedCreationTypeLabel = discInAuthorLabel;
            discInAuthorLabel.Click += (sender, e) => CreationType = NoteCreationType.DiscInAuthor;
            authorInDiscLabel.Click += (sender, e) => CreationType = NoteCreationType.AuthorInDisc;
            allInputs.ForEach(x => x.InputNameBox.CheckValid());
        }
        private void SetupLayout()
        {
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

            SetupButtons();
            SetupDragDrop();
            SetupInputs();


        }
        private void SetupInputs()
        {
            SetupInputsOrder();
            ClearInputEvents();

            if (InputDataOrdered.First == null) throw new Exception("Something went in Input Field Organization Setup");

            if (CreationType == NoteCreationType.DiscInAuthor)
                InputDataOrdered.First.Value.SetDataSource<NoteAuthor>(MainForm.Container);
            else
                InputDataOrdered.First.Value.SetDataSource<NoteDisc>(MainForm.Container);

            foreach (var inp in InputDataOrdered)
            {
                if (inp == InputDataOrdered.First.Value)
                    inp.InputNameBox.CanBeEmpty = false;
                else
                    inp.InputNameBox.CanBeEmpty = true;
                inp.InputNameBox.Text = "";
            }

            SetupEventOrder();

            
            SetupInputEvents();

            foreach (var input in allInputs)
            {
                input.SetLabelFont(new Font(Font.FontFamily, 18, GraphicsUnit.Point));
                input.InputNameBox.CheckValid();
            }
        }
        private void ClearInputEvents()
        {
            foreach (var inp in InputDataOrdered)
                inp.InputNameBox.ClearEvents();
        }
        private void SetupInputEvents()
        {
            ClearInputEvents();
            if (InputEventsOrdered.First != null && InputDataOrdered.First != null)
            {
                var action = InputEventsOrdered.First;
                for (var input = InputDataOrdered.First; input != null && action != null; action = action.Next, input = input.Next)
                {
                    input.Value.InputNameBox.TempStatusChangedRepeatedly += new StateChangedEvent(action.Value);
                }
            }
        }
        private void SetupEventOrder()
        {
            InputEventsOrdered.Clear();
            InputEventsOrdered.AddLast(MainInputStateChanged);
            InputEventsOrdered.AddLast(SecondaryInputStateChanged);
            InputEventsOrdered.AddLast(SongInputStateChanged);
        }
        private void MainInputStateChanged(SmartComboBox box, InputState state)
        {
            var secondary = InputDataOrdered?.First?.Next?.Value;
            if (secondary == null) return;

            if (state == InputState.OK)
            {
                var data = (
                    box.InnerData.Find(x => x.NoteName == box.Text) as NoteControlParent
                );
                if (data != null)
                {
                    if (CreationType == NoteCreationType.DiscInAuthor)
                        secondary.SetDataSource<NoteDisc>(data);
                    else
                        secondary.SetDataSource<NoteAuthor>(data);
                    InputSong.SetDataSource<NoteSong>(data);
                }
            }
            else
            {
                secondary.ClearDataSource();
                InputSong.ClearDataSource();
            }

            secondary.InputNameBox.CheckValid();
            InputSong.InputNameBox.CheckValid();
        }
        private void SecondaryInputStateChanged(SmartComboBox box, InputState state)
        {
            var dataSelf = (
                box.InnerData.Find(x => x.NoteName == box.Text) as NoteControlMidder
            );

            if (state == InputState.OK)
            {
                var dataSongs = dataSelf?.InnerNotes;
                if (dataSelf != null)
                {
                    InputSong.SetDataSource<NoteSong>(dataSelf);
                }
            }
            else
            {
                if (box.NoteParent != null)
                    InputSong.SetDataSource<NoteSong>(box.NoteParent);
            }
            InputSong.InputNameBox.CheckValid();

        }
        private void SongInputStateChanged(SmartComboBox box, InputState state)
        {
            if (state == InputState.OK)
            {
                var dataSongFiles = (
                    box.InnerData.Find(x => x.NoteName == box.Text) as NoteControlParent
                );
                Debug.WriteLine($"Event state data is : \n{dataSongFiles}");
                if (dataSongFiles != null)
                {
                    InputSongFile.SetDataSource<NoteSongFile>(dataSongFiles);
                }
            }
            else
            {
                InputSongFile.ClearDataSource();
            }
            if (state == InputState.UNKNOWN)
            {
                InputSongFile.Enabled = false;
                InputSongFile.Clean();
            }
            else
                InputSongFile.Enabled = true;

            InputSongFile.InputNameBox.CheckValid();
        }

        private void SetupDragDrop()
        {
            dragDropPanel.BackColor = MainForm.title.BackColor;
            dragDropText.BackColor = ControlPaint.LightLight(MainForm.title.BackColor);
            dragDropText.Font = createButton.Font;
            dragDropText.AllowDrop = true;
            dragDropText.DragEnter += (sender, e) =>
            {
                if (!IsDropDataValid(e))
                    return;
                if (
                    (e.Data?.GetData(DataFormats.FileDrop) as string[])?
                        .Where(x => x.Contains(".mp3"))
                        .Count() > 0
                )
                {
                    e.Effect = DragDropEffects.Link;
                    return;
                }

                e.Effect = DragDropEffects.None;
            };
            dragDropText.DragDrop += (sender, e) =>
            {
                if (!IsDropDataValid(e))
                    return;
                if (e.Effect != DragDropEffects.Link)
                    return;

                var mp3 = ((string[])e.Data.GetData(DataFormats.FileDrop))
                    .Where(x => x.Contains(".mp3"))
                    .ToArray()[0];

                var fileData = MusicFileScanner.GetDataFromFile(mp3);
                FillWithData(fileData);
            };
        }
        private void FillWithData(Dictionary<InputType, (string? Name, string? Description)> data)
        {
            if (InputDataOrdered.Select(x => x.InputNameBox.Status).Where(x => x == InputState.OK || x == InputState.CREATION).Count() > 1)
            {
                var ask = MessageBox.Show("There is already some info in the fields. Would you like to override it (YES) or load only Song File (NO) ?", "File data overrideness", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (ask == DialogResult.Cancel) return;

                if (ask == DialogResult.No)
                    data = data.Where(x => x.Key == InputType.SongFile).ToDictionary(x => x.Key, v => v.Value);
            }

            foreach (var kp in data)
            {
                var input = allInputs.Find(x => x.InputType == kp.Key);
                if (input == null) continue;

                if (kp.Value.Name != null)
                    input.InputNameBox.Text = kp.Value.Name;
                if (kp.Value.Description != null)
                    input.InputDescriptionBox.Text = kp.Value.Description;
                input.InputNameBox.CheckValid();
            }

        }

        private bool IsDropDataValid(DragEventArgs e) => e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);
        private void SetupButtons()
        {
            createButton.Size = new Size(0, 70);
            createButton.Font = new Font(Font.FontFamily, 50, GraphicsUnit.Pixel);
            createButton.BackColor = MainForm.title.BackColor;
            createButton.FlatAppearance.BorderColor = ControlPaint.DarkDark(createButton.BackColor);
            createButton.FlatAppearance.BorderSize = 4;
            createButton.Click += (sender, e) =>
            {
                NoteBuilder creator;
                try
                {
                    creator = new NoteBuilder(
                        MainForm,
                        new LinkedList<OutputInfo>(InputDataOrdered.Select(x => x.GetOutput()))
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
        }


    }
}
