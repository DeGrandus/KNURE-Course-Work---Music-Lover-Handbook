﻿using MusicLoverHandbook.Controller;
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

namespace MusicLoverHandbook.View.Forms
{
    public partial class NoteCreationMenu : Form
    {
        public LinkedList<InputData> InputDataOrdered = new();
        public LinkedList<Action<SmartComboBox, InputStatus>> InputEventsOrdered = new();
        private NoteCreationOrder creationOrder = NoteCreationOrder.AuthorThenDisc;
        private Label selectedCreationTypeLabel;

        public NoteCreationMenu(MainForm mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
            InputDataOrdered = new LinkedList<InputData>();
            SetupSwitchButtons();
            SetupLayout();
        }

        public NoteCreationOrder CreationType
        {
            get => creationOrder;
            set
            {
                if (creationOrder == value)
                    return;

                if (value is NoteCreationOrder.AuthorThenDisc)
                    SelectedCreationTypeLabel = discInAuthorLabel;
                else
                    SelectedCreationTypeLabel = authorInDiscLabel;

                creationOrder = value;
                SetupInputs();
            }
        }

        public NoteControlMidder? FinalNote { get; private set; }
        public MainForm MainForm { get; }
        private List<InputData> allInputs => InputDataOrdered.ToList();

        private Label SelectedCreationTypeLabel
        {
            get => selectedCreationTypeLabel;
            set
            {
                if (selectedCreationTypeLabel != value)
                    (selectedCreationTypeLabel.BackColor, value.BackColor) = (
                        value.BackColor,
                        selectedCreationTypeLabel.BackColor
                    );
                selectedCreationTypeLabel = value;
            }
        }

        private void ClearInputEvents()
        {
            foreach (var inp in InputDataOrdered)
                inp.InputNameBox.ClearEvents();
        }

        private void FillWithData(Dictionary<NoteType, (string? Name, string? Description)> data)
        {
            if (
                InputDataOrdered
                    .Select(x => x.InputNameBox.Status)
                    .Where(x => x == InputStatus.OK || x == InputStatus.CREATION)
                    .Count() > 1
            )
            {
                var ask = MessageBox.Show(
                    "There is already some info in the fields. Would you like to override it (YES) or load only Song File (NO) ?",
                    "File data overrideness",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );
                if (ask == DialogResult.Cancel)
                    return;

                if (ask == DialogResult.No)
                    data = data.Where(x => x.Key == NoteType.SongFile)
                        .ToDictionary(x => x.Key, v => v.Value);
            }

            foreach (var kp in data)
            {
                var input = allInputs.Find(x => x.InputType == kp.Key);
                if (input == null)
                    continue;

                if (kp.Value.Name != null)
                    input.InputNameBox.Text = kp.Value.Name;
                if (kp.Value.Description != null)
                    input.InputDescriptionBox.Text = kp.Value.Description;
                input.InputNameBox.CheckValid();
            }
        }

        private bool IsDropDataValid(DragEventArgs e) =>
            e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);

        private void MainInputStateChanged(SmartComboBox box, InputStatus state)
        {
            var secondary = InputDataOrdered?.First?.Next?.Value;
            if (secondary == null)
                return;

            if (state == InputStatus.OK)
            {
                var data = (box.InnerData.Find(x => x.NoteName == box.Text) as NoteControlParent);
                if (data != null)
                {
                    if (CreationType == NoteCreationOrder.AuthorThenDisc)
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

        private void SecondaryInputStateChanged(SmartComboBox box, InputStatus state)
        {
            var dataSelf = (box.InnerData.Find(x => x.NoteName == box.Text) as NoteControlMidder);

            if (state == InputStatus.OK)
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

        private void SetupButtons()
        {
            createButton.Size = new Size(0, 70);
            createButton.Font = new Font(Font.FontFamily, 50, GraphicsUnit.Pixel);
            createButton.BackColor = MainForm.title.BackColor;
            createButton.FlatAppearance.BorderColor = ControlPaint.DarkDark(createButton.BackColor);
            createButton.FlatAppearance.BorderSize = 4;
            createButton.Click += (sender, e) =>
            {
                NoteBuilder creator = new NoteBuilder(MainForm, creationOrder);
                NoteControlMidder FinalNote;
                try
                {
                    FinalNote = creator.CreateNote(new LinkedList<OutputInfo>(InputDataOrdered.Select(x => x.GetOutput())));
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
                DialogResult = DialogResult.OK;
                Close();
            };
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
                    (e.Data?.GetData(DataFormats.FileDrop) as string[])
                        ?.Where(x => x.Contains(".mp3"))
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

                var mp3 = (e.Data?.GetData(DataFormats.FileDrop) as string[])
                    ?.Where(x => x.Contains(".mp3"))
                    .ToList()
                    .Find(x => x.Contains(".mp3"));

                if (mp3 == null)
                    return;
                var fileData = MusicFileScanner.GetDataFromFile(mp3);
                FillWithData(fileData);
            };
        }

        private void SetupEventOrder()
        {
            InputEventsOrdered.Clear();
            InputEventsOrdered.AddLast(MainInputStateChanged);
            InputEventsOrdered.AddLast(SecondaryInputStateChanged);
            InputEventsOrdered.AddLast(SongInputStateChanged);
        }

        private void SetupInputEvents()
        {
            ClearInputEvents();
            if (InputEventsOrdered.First != null && InputDataOrdered.First != null)
            {
                var action = InputEventsOrdered.First;
                for (
                    var input = InputDataOrdered.First;
                    input != null && action != null;
                    action = action.Next, input = input.Next
                )
                {
                    input.Value.InputNameBox.TempStatusChangedRepeatedly += new StateChangedEvent(
                        action.Value
                    );
                }
            }
        }

        private void SetupInputs()
        {
            SetupInputsOrder();
            ClearInputEvents();

            if (InputDataOrdered.First == null)
                throw new Exception("Something went in Input Field Organization Setup");

            if (CreationType == NoteCreationOrder.AuthorThenDisc)
                InputDataOrdered.First.Value.SetDataSource<NoteAuthor>(MainForm.NotesContainer);
            else
                InputDataOrdered.First.Value.SetDataSource<NoteDisc>(MainForm.NotesContainer);

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

        private void SetupInputsOrder()
        {
            InputDataOrdered.Clear();
            InputData main = InputAuthor,
                secondary = InputDisc;
            if (CreationType == NoteCreationOrder.DiscThenAuthor)
                (main, secondary) = (secondary, main);
            InputDataOrdered.AddLast(main);
            InputDataOrdered.AddLast(secondary);
            InputDataOrdered.AddLast(InputSong);
            InputDataOrdered.AddLast(InputSongFile);
            for (var inp = InputDataOrdered.First; inp != null; inp = inp.Next)
                tableInputs.Controls.Add(inp.Value, 0, allInputs.IndexOf(inp.Value));
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

        private void SetupSwitchButtons()
        {
            selectedCreationTypeLabel = discInAuthorLabel;
            discInAuthorLabel.Click += (sender, e) =>
                CreationType = NoteCreationOrder.AuthorThenDisc;
            authorInDiscLabel.Click += (sender, e) =>
                CreationType = NoteCreationOrder.DiscThenAuthor;
            allInputs.ForEach(x => x.InputNameBox.CheckValid());
        }

        private void SongInputStateChanged(SmartComboBox box, InputStatus state)
        {
            if (state == InputStatus.OK)
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
            if (state == InputStatus.UNKNOWN)
            {
                InputSongFile.Enabled = false;
                InputSongFile.Clean();
            }
            else
                InputSongFile.Enabled = true;

            InputSongFile.InputNameBox.CheckValid();
        }
    }
}