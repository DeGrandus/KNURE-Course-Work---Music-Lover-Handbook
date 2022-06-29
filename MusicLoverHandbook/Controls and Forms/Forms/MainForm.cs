using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Logic.Notes;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Properties;
using MusicLoverHandbook.View.Forms;
using System.ComponentModel;
using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    public partial class MainForm : Form
    {
        #region Public Fields

        public Color ContentBackColor;
        public Color LabelBackColor;

        #endregion Public Fields



        #region Public Properties

        public NoteBuilder Builder { get; }
        public RawNoteManager NoteManager { get; }

        public NotesContainer NotesContainer { get; }

        #endregion Public Properties



        #region Public Constructors

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            MinimumSize = new Size(300, 565);
            Builder = new(this);
            NoteManager = new(this);
            NotesContainer = new NotesContainer(contentPanel.MovingContentBox, qSTextBox, qSSwitchLabel);

            Debug.WriteLine(FileManager.Instance.DataFilePath);

            SetupLayout();
        }

        #endregion Public Constructors



        #region Private Methods

        private void AdaptToSize()
        {
            if (Size.Width < MinimumSize.Width || Size.Height < MinimumSize.Height)
                return;
            int rmax = mainLayoutTable.RowCount - 1,
                cmax = mainLayoutTable.ColumnCount - 1;
            var tb = mainLayoutTable;

            var wDiff = Size.Width - MinimumSize.Width;
            var hDiff = Size.Height - MinimumSize.Height;

            var wLim = 150;
            tb.ColumnStyles[0].Width = (wDiff <= wLim ? ((float)wDiff / wLim) : 1) * 20;
            tb.ColumnStyles[cmax].Width = (wDiff <= wLim ? ((float)wDiff / wLim) : 1) * 20;
            wLim = 400;
            tb.ColumnStyles[1].Width = (wDiff <= wLim ? ((float)wDiff / wLim) : 1) * 100;
            tb.ColumnStyles[cmax - 1].Width = (wDiff <= wLim ? ((float)wDiff / wLim) : 1) * 100;
            var hLim = 100;
            tb.RowStyles[rmax].Height = (hDiff <= hLim ? ((float)hDiff / hLim) : 1) * 20;
            tb.RowStyles[rmax - 2].Height = (hDiff <= hLim ? ((float)hDiff / hLim) : 1) * 50;

            tb.RowStyles[0].Height = (hDiff <= hLim / 2 ? ((float)hDiff / (hLim / 2)) : 1) * 50;

            tb.RowStyles[1].Height =
                (
                    hDiff <= hLim * 2
                        ? hDiff <= hLim
                            ? 0
                            : (((float)hDiff - 100) / hLim)
                        : 1
                ) * 50;

            mainLayoutTable.ColumnStyles[2].Width =
                wDiff <= 180 ? 100 + (float)wDiff / 180 * 100 : 200;
            mainLayoutTable.ColumnStyles[4].Width =
                wDiff <= 180 ? 100 + (float)wDiff / 180 * 100 : 200;
            if (wDiff < 250)
            {
                if (wDiff < 140)
                {
                    sortStripButton.Margin = new(0, 3, 0, 3);
                    advFilterButton.Margin = new(0, 3, 0, 3);
                }
                if (wDiff < 80)
                    qSSwitchLabel.Text = "QS: ";
                else
                    qSSwitchLabel.Text = "QSearch: ";
            }
            else
            {
                sortStripButton.Margin = new(3);
                advFilterButton.Margin = new(3);
                qSSwitchLabel.Text = "Quick search: ";
                qSSwitchLabel.Size = new(150, 1);
            }
            if (wDiff < 60)
            {
                saveButton.Text = "S";
                saveButton.TextAlign = ContentAlignment.MiddleRight;
                loadButton.Text = "L";
                loadButton.TextAlign = ContentAlignment.MiddleRight;
            }
            else
            {
                saveButton.Text = "Save";
                saveButton.TextAlign = ContentAlignment.MiddleCenter;
                loadButton.Text = "Load";
                loadButton.TextAlign = ContentAlignment.MiddleCenter;
            }
            using (var g = Graphics.FromHwnd(qSSwitchLabel.Handle))
                qSSwitchLabel.Size = new(
                    (int)g.MeasureString(qSSwitchLabel.Text, qSSwitchLabel.Font).Width + 5,
                    1
                );
        }

        private void AdvancedSearchButton_Click(object? sender, EventArgs e)
        {
            var filterMenu = new NoteAdvancedFilterMenu(this);

            if (filterMenu.ShowDialog() == DialogResult.OK)
            {
                var final = filterMenu.FinalizedOutput;
                foreach (var note in final)
                    note.InvokeActionHierarcaly(
                        n =>
                        {
                            n.IsEditShown = false;
                            n.IsDeleteShown = false;
                        }
                    );
                NotesContainer.AdvancedFilteredNotes = filterMenu.FinalizedOutput;
            }
        }

        private Font ConvertToDesiredHeight(Font font, int h)
        {
            return new Font(font.FontFamily, h, font.Style, GraphicsUnit.Pixel);
        }

        private Color[] CreateGradient()
        {
            var step = 10;
            var colors = new[]
            {
                Color.FromArgb(255, Color.FromArgb(0x3056E0)),
                Color.FromArgb(255, Color.FromArgb(0xCFD9FE)),
                Color.FromArgb(255, Color.FromArgb(0x3056E0)),
            };
            var groups = new List<(Color from, Color to)>();
            colors.Aggregate(
                (c, x) =>
                {
                    groups.Add((c, x));
                    return x;
                }
            );

            var gradiented = new List<Color>();
            groups.ForEach(
                x =>
                {
                    int fR = x.from.R,
                        fG = x.from.G,
                        fB = x.from.B;
                    int tR = x.to.R,
                        tG = x.to.G,
                        tB = x.to.B;

                    int sR = (tR - fR);
                    int sG = (tG - fG);
                    int sB = (tB - fB);

                    step = (Math.Abs(sR) + Math.Abs(sG) + Math.Abs(sB)) / 4;

                    for (double i = 0; i < 1; i += 1.0 / step)
                    {
                        gradiented.Add(
                            Color.FromArgb(
                                255,
                                (int)Math.Round(fR + sR * i),
                                (int)Math.Round(fG + sG * i),
                                (int)Math.Round(fB + sB * i)
                            )
                        );
                    }
                }
            );
            return gradiented.ToArray();
        }

        private void CreateNoteButton_Click(object? sender, EventArgs e)
        {
            var controller = new NoteCreationMenuController(this);

            var creationResult = controller.OpenCreationMenu();
            SuspendLayout();
            creationResult?.CreateNote();
            NotesContainer.InvokeQuickSearch();

            FileManager.Instance.HistoryManager.UpdateHistory(NotesContainer);

            ResumeLayout();
        }

        private void FillWithNew(IEnumerable<INoteControlChild> newNotes)
        {
            NotesContainer.InnerNotes.Clear();
            foreach (var note in newNotes)
                NotesContainer.InnerNotes.Add(note);
        }

        private void LoadButton_Click(object? sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON file|*.json";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.InitialDirectory = Path.GetFileNameWithoutExtension(
                FileManager.Instance.DataFilePath
            );
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var notes = FileManager.Instance.RecreateNotesFromData(openFileDialog.FileName);
                FillWithNew(notes.OfType<INoteControlChild>());
                FileManager.Instance.HistoryManager.UpdateHistory(NotesContainer);
            }
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JSON file|*.json";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.InitialDirectory = Path.GetFileNameWithoutExtension(
                    FileManager.Instance.DataFilePath
                );
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var defaultQuestion = MessageBox.Show(
                        "Do you want to make this file default?\n(non-shift & on-close save etc. will store data in it)\n(\"cancel\" cancels saving operation)",
                        "Default save file",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question
                    );
                    if (defaultQuestion == DialogResult.Cancel)
                        return;

                    if (defaultQuestion == DialogResult.Yes)
                        FileManager.Instance.SetDataPath(saveFileDialog.FileName);

                    FileManager.Instance.WriteToDataFile(NotesContainer, saveFileDialog.FileName);
                }
            }
            else
                FileManager.Instance.WriteToDataFile(NotesContainer);
        }

        private void Setup_AdvancedSearchButton()
        {
            advFilterButton.BackColor = ControlPaint.Light(advFilterButton.Parent.BackColor);
            advFilterButton.FlatAppearance.BorderColor = ControlPaint.Dark(
                advFilterButton.Parent.BackColor,
                0.2f
            );
            advFilterButton.FlatAppearance.BorderSize = 1;
            advFilterButton.Click += AdvancedSearchButton_Click;
        }

        private void Setup_BasicTooltips()
        {
            var tooltip = new ToolTip()
            {
                IsBalloon = true,
                ToolTipIcon = ToolTipIcon.Info,
                ToolTipTitle = "Information"
            };
            tooltip.SetToolTip(loadButton, "Loads notes from choosen file");
            tooltip.SetToolTip(undoButton, "Undo latest action");
            tooltip.SetToolTip(redoButton, "Redo latest action");
            tooltip.SetToolTip(
                cancelFilteringButton,
                "Resets view to all notes. \n( Needs two clicks )"
            );
            tooltip.SetToolTip(advFilterButton, "Open Advanced Filtering menu");
            tooltip.SetToolTip(
                sortStripButton,
                "Opens strip menu for sorting.\n> applyed sorting also will be performed after any change of view automaticly;\n> sorting will not affect initial notes structure;"
            );
            tooltip.SetToolTip(createNoteButton, "Opens menu for creating new notes");
        }

        private void Setup_CancelFilterButton()
        {
            cancelFilteringButton.Click += (sender, e) =>
            {
                if (cancelFilteringButton.Tag == null)
                    cancelFilteringButton.Tag = 0;
                cancelFilteringButton.Tag = (int)cancelFilteringButton.Tag + 1;
                var ind = (int)cancelFilteringButton.Tag;
                if (ind == 1)
                {
                    cancelFilteringButton.BackColor = Color.FromArgb(
                        255,
                        ControlPaint.Light(Color.Yellow)
                    );
                    Task.Run(
                        () =>
                        {
                            if ((int)cancelFilteringButton.Tag != 1)
                                return;
                            Thread.Sleep(1000);
                            Setup_CancelFilterButton_BasicAppearance();
                            cancelFilteringButton.Tag = 0;
                        }
                    );
                }
                else
                {
                    cancelFilteringButton.BackColor = Color.FromArgb(
                        255,
                        ControlPaint.Light(Color.Red)
                    );
                    cancelFilteringButton.FlatAppearance.BorderColor = Color.FromArgb(
                        255,
                        ControlPaint.Light(Color.Red, -4f)
                    );
                    cancelFilteringButton.FlatAppearance.BorderSize = 1;
                    cancelFilteringButton.Tag = 0;
                    cancelFilteringButton.Enabled = false;
                    Task.Run(
                        () =>
                        {
                            Thread.Sleep(200);
                            Setup_CancelFilterButton_BasicAppearance();
                            cancelFilteringButton.Enabled = true;
                        }
                    );
                    NotesContainer.AdvancedFilteredNotes = null;
                }
            };
            cancelFilteringButton.Text = "X";
        }

        private void Setup_CancelFilterButton_BasicAppearance()
        {
            var cancelTheme = title.BackColor;
            cancelFilteringButton.BackColor = Color.FromArgb(255, ControlPaint.Light(cancelTheme));
            cancelFilteringButton.FlatAppearance.BorderColor = Color.FromArgb(
                255,
                ControlPaint.Light(cancelTheme, -4f)
            );
            cancelFilteringButton.FlatAppearance.BorderSize = 1;
        }

        private void Setup_CreateNoteButton_AnimationWorker()
        {
            var buttonGradientWorker = new BackgroundWorker();
            buttonGradientWorker.DoWork += (sender, e) =>
            {
                var colors = CreateGradient();
                var ind = 0;
                while (true)
                {
                    if (ind + 1 >= colors.Length)
                        ind = 1;
                    createNoteButton.FlatAppearance.BorderColor = colors[ind];
                    createNoteButton.FlatAppearance.MouseDownBackColor = ControlPaint.LightLight(
                        ControlPaint.Light(colors[ind], 1f)
                    );
                    createNoteButton.FlatAppearance.MouseOverBackColor = ControlPaint.LightLight(
                        ControlPaint.Light(colors[ind], 0.5f)
                    );
                    createNoteButton.BackColor = ControlPaint.LightLight(
                        ControlPaint.LightLight(colors[ind])
                    );
                    ind++;
                    Thread.Sleep(1);
                }
            };
            buttonGradientWorker.RunWorkerAsync();
            Resize += (sender, e) =>
            {
                AdaptToSize();
            };
            Load += (sender, e) =>
            {
                AdaptToSize();
            };
        }

        private void Setup_CreateNoteButton_Base()
        {
            createNoteButton.FlatAppearance.BorderSize = 2;
            createNoteButton.Text = "Create new Note";
            createNoteButton.Click += CreateNoteButton_Click;
        }

        private void Setup_QSSwitch()
        {
            qSSwitchLabel.BasicTooltipText = "[ Match only names ]";
            qSSwitchLabel.SpecialTooltipText = "[ Match both names and descriptions ]";
            qSSwitchLabel.BackColorChanged += (sender, e) =>
            {
                qSPanel.BackColor = qSSwitchLabel.BackColor;
            };
        }

        private void Setup_ReassignFonts()
        {
            Font = new Font(FontContainer.Instance.LoadedFamilies[0], 15);
            title.Font = ConvertToDesiredHeight(Font, title.Height);
            createNoteButton.Font = Font;
        }

        private void Setup_SaveButtonStateTimer()
        {
            var saveButtonModeTimer = new Timer() { Interval = 1, Enabled = false };
            saveButtonModeTimer.Tick += (sender, e) =>
            {
                saveButton.BackColor =
                    ModifierKeys == Keys.Shift
                        ? ControlPaint.Light(Color.FromArgb(255, Color.FromArgb(0xEEE82C)))
                        : sortStripButton.BackColor;
                if (saveButton.Text == "S")
                    return;
                saveButton.Text = ModifierKeys == Keys.Shift ? "Save As" : "Save";
            };

            saveButton.MouseEnter += (sender, e) =>
            {
                saveButtonModeTimer.Start();
            };
            saveButton.MouseLeave += (sender, e) =>
            {
                saveButtonModeTimer.Stop();
                saveButton.BackColor = sortStripButton.BackColor;
            };
        }

        private void Setup_SaveLoadButtons()
        {
            var getIcoRect = (Button bt) => new Rectangle(0, 3, bt.Height - 6, bt.Height - 6);
            saveButton.Paint += (sender, e) =>
                e.Graphics.DrawImage(Resources.DownloadIcon, getIcoRect((Button)sender!));
            loadButton.Paint += (sender, e) =>
                e.Graphics.DrawImage(Resources.UploadIcon, getIcoRect((Button)sender!));
            saveButton.Click += SaveButton_Click;
            loadButton.Click += LoadButton_Click;
            Setup_SaveButtonStateTimer();
            saveButton.BackColor = sortStripButton.BackColor;
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.Font = new Font(
                FontContainer.Instance.LoadedFamilies[0],
                saveButton.Height * 2 / 3,
                GraphicsUnit.Pixel
            );
            loadButton.BackColor = sortStripButton.BackColor;
            loadButton.FlatStyle = FlatStyle.Flat;
            loadButton.Font = new Font(
                FontContainer.Instance.LoadedFamilies[0],
                saveButton.Height * 2 / 3,
                GraphicsUnit.Pixel
            );
        }

        private void Setup_SettingsButton()
        {
            Load += (sender, e) =>
                settingsButton.Size = new(settingsButton.Height, settingsButton.Height);
            settingsButton.BackgroundImage = Resources.SettingsIcon;
            settingsButton.BackgroundImageLayout = ImageLayout.Zoom;
            settingsButton.FlatAppearance.BorderSize = 0;
            settingsButton.FlatStyle = FlatStyle.Flat;
            var motionLimits = (min: 0, curr: 0, max: 45);
            var motionTimer = new Timer() { Interval = 1, Tag = 1 };
            settingsButton.Click += (sender, e) =>
            {
                new SettingsMenu(this).ShowDialog();
            };

            settingsButton.MouseEnter += (sender, e) =>
            {
                motionTimer.Enabled = true;
                motionTimer.Tag = 1;
            };
            settingsButton.MouseLeave += (sender, e) => motionTimer.Tag = -1;

            motionTimer.Tick += (sender, e) =>
            {
                var inc = (int)motionTimer.Tag!;
                if (inc == -1 && motionLimits.curr == 0)
                {
                    motionTimer.Stop();
                    return;
                }
                motionLimits.curr += inc;
                motionLimits.curr =
                    motionLimits.curr >= motionLimits.max ? motionLimits.max : motionLimits.curr;
                motionLimits.curr =
                    motionLimits.curr <= motionLimits.min ? motionLimits.min : motionLimits.curr;
                var index =
                    ((float)motionLimits.curr) / (float)(motionLimits.max - motionLimits.min);
                var scaling = (float)((1 - (Math.Pow(1 - Math.Abs(index * 2 - 1), 3))) * 0.2 + 0.8);
                var placement = Resources.SettingsIcon;
                var image = new Bitmap(placement.Width, placement.Height);
                using (var g = Graphics.FromImage(image))
                {
                    g.TranslateTransform(((float)image.Width) / 2, ((float)image.Height) / 2);
                    g.RotateTransform(
                        (float)((Math.Sin(-Math.PI / 2 + Math.PI * index) + 1) / 2 * 270)
                    );

                    g.ScaleTransform(scaling, scaling);
                    g.TranslateTransform(-((float)image.Width) / 2, -((float)image.Height) / 2);

                    g.DrawImage(placement, new Rectangle(0, 0, image.Width, image.Height));
                }
                Debug.WriteLine(index * 180);
                settingsButton.BackgroundImage = image;
                settingsButton.BackgroundImageLayout = ImageLayout.Zoom;
            };
        }

        private void Setup_SortingStripButton_Base()
        {
            sortStripButton.BackColor = ControlPaint.Light(advFilterButton.Parent.BackColor);
            sortStripButton.FlatAppearance.BorderColor = ControlPaint.Dark(
                advFilterButton.Parent.BackColor,
                0.2f
            );
            sortStripButton.FlatAppearance.BorderSize = 1;
            sortStripButton.ImageStripColor = advFilterButton.Parent.BackColor;
        }

        private void Setup_SortingStripButton_MenuStrip()
        {
            var strip = new ContextMenuStrip() { };
            var alphabeticalSortRadio = new RadioButton()
            {
                Text = "Alphabeticaly",
                Tag = (IEnumerable<INoteControlChild> children) => children.OrderBy(x => x.NoteName)
            };
            var contentSortRadio = new RadioButton()
            {
                Text = "By content amount",
                Tag = (IEnumerable<INoteControlChild> children) =>
                    children.OrderBy(x => x is IParentControl p ? p.InnerNotes.Count : 0)
            };
            var radioGroup = new[] { alphabeticalSortRadio, contentSortRadio }.ToList();
            var reversiveSortCheck = new CheckBox()
            {
                Text = "Reverse",
                Tag = (IEnumerable<INoteControlChild> children) =>
                {
                    var t = children.ToArray().Reverse();

                    Debug.WriteLine("REVERSE BLOCK");
                    Debug.WriteLine(string.Join(", ", children));
                    Debug.WriteLine("------------");
                    Debug.WriteLine(string.Join(", ", t));
                    Debug.WriteLine("REVERSE BLOCK END");
                    return t;
                }
            };
            var applyFilteringButton = new ToolStripButton() { Text = "Apply sorting" };
            applyFilteringButton.Click += (sender, e) =>
            {
                var filter = radioGroup.FirstOrDefault(x => x.Checked)?.Tag;
                var reverse = reversiveSortCheck.Checked ? reversiveSortCheck.Tag : null;
                var filtersConverted = new List<object?>() { filter, reverse }.OfType<
                    Func<IEnumerable<INoteControlChild>, IEnumerable<INoteControlChild>>
                >();
                Debug.WriteLine(string.Join(", ", filtersConverted));
                NotesContainer.Filters = filtersConverted.ToList();
            };
            var clearFilteringButton = new ToolStripButton() { Text = "Clear filters" };
            clearFilteringButton.Click += (sender, e) =>
            {
                radioGroup.ForEach(x => x.Checked = false);
                reversiveSortCheck.Checked = false;
                NotesContainer.Filters = new();
            };

            strip.Items.Add(new ToolStripControlHost(alphabeticalSortRadio));
            strip.Items.Add(new ToolStripControlHost(contentSortRadio));
            strip.Items.Add(new ToolStripControlHost(reversiveSortCheck));
            strip.Items.Add(new ToolStripSeparator());
            strip.Items.Add(applyFilteringButton);
            strip.Items.Add(clearFilteringButton);
            sortStripButton.MenuStrip = strip;
            strip.PerformLayout();
        }

        private void Setup_UndoRedoButtons()
        {
            undoButton.BackColor = ControlPaint.Light(advFilterButton.Parent.BackColor);
            undoButton.FlatAppearance.BorderColor = ControlPaint.Dark(undoButton.BackColor, 0.75f);
            undoButton.FlatAppearance.BorderSize = 1;
            undoButton.BackgroundImage = Resources.UndoIcon;
            undoButton.BackgroundImageLayout = ImageLayout.Zoom;

            redoButton.BackColor = ControlPaint.Light(advFilterButton.Parent.BackColor);
            redoButton.FlatAppearance.BorderColor = ControlPaint.Dark(undoButton.BackColor, 0.75f);
            redoButton.FlatAppearance.BorderSize = 1;
            var redo = new Bitmap(Resources.UndoIcon);
            redo.RotateFlip(RotateFlipType.RotateNoneFlipX);
            redoButton.BackgroundImage = redo;
            redoButton.BackgroundImageLayout = ImageLayout.Zoom;

            undoButton.Click += (sender, e) =>
            {
                if (FileManager.Instance.HistoryManager.UndoNotes() is List<NoteControl> newNotes)
                    FillWithNew(newNotes.OfType<INoteControlChild>());
            };
            redoButton.Click += (sender, e) =>
            {
                if (FileManager.Instance.HistoryManager.RedoNotes() is List<NoteControl> newNotes)
                    FillWithNew(newNotes.OfType<INoteControlChild>());
            };
        }

        private void SetupLayout()
        {
            var mainColor = Color.FromArgb(0x768DE2);
            LabelBackColor = ControlPaint.LightLight(Color.FromArgb(255, mainColor));
            topPanel.BackColor = LabelBackColor;
            mainLayoutTable.BackColor = Color.White;
            ContentBackColor = ControlPaint.Light(Color.FromArgb(255, mainColor), 1.5f);
            contentPanel.BackColor = ContentBackColor;
            searchBarLayout.BackColor = LabelBackColor;

            noteContentTable.RowStyles[0].Height = 50;
            toolsTable.ColumnStyles[2].Width = 50;

            Setup_CreateNoteButton_Base();
            Setup_CreateNoteButton_AnimationWorker();

            Setup_SortingStripButton_Base();
            Setup_SortingStripButton_MenuStrip();

            Setup_QSSwitch();
            Setup_CancelFilterButton();
            Setup_AdvancedSearchButton();
            Setup_SaveLoadButtons();
            Setup_UndoRedoButtons();
            Setup_SettingsButton();

            Setup_BasicTooltips();

            Setup_ReassignFonts();
        }

        #endregion Private Methods
    }
}
