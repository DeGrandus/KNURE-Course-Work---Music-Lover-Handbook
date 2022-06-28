using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Logic.Notes;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Properties;
using System.ComponentModel;
using System.Diagnostics;

namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    public partial class MainForm : Form
    {
        public Color ContentBackColor;
        public Color LabelBackColor;
        public NoteBuilder Builder { get; }
        public RawNoteManager NoteManager { get; }

        public NotesContainer NotesContainer { get; }

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            MinimumSize = new Size(300, 565);
            Builder = new(this);
            NoteManager = new(this);
            NotesContainer = new NotesContainer(contentPanel, qSTextBox, qSSwitchLabel);

            Debug.WriteLine(FileManager.Instance.DataFilePath);

            SetupLayout();
        }

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
            if (wDiff < 180)
            {
                sortStripButton.Margin = new(0, 3, 0, 3);
                advFilterButton.Margin = new(0, 3, 0, 3);
                qSSwitchLabel.Text = "QS: ";
                saveButton.Dock = DockStyle.Left;
                loadButton.Dock = DockStyle.Right;
                saveButton.Text = "S";
                saveButton.TextAlign = ContentAlignment.MiddleRight;
                saveButton.Size = new(
                    (int)mainLayoutTable.RowStyles[1].Height + 20,
                    (int)mainLayoutTable.RowStyles[1].Height
                );
                loadButton.Text = "L";
                loadButton.TextAlign = ContentAlignment.MiddleRight;
                loadButton.Size = new(
                    (int)mainLayoutTable.RowStyles[1].Height + 20,
                    (int)mainLayoutTable.RowStyles[1].Height
                );
            }
            else
            {
                sortStripButton.Margin = new(3);
                advFilterButton.Margin = new(3);
                qSSwitchLabel.Text = "Quick search: ";
                qSSwitchLabel.Size = new(150, 1);
                saveButton.Dock = DockStyle.Fill;
                loadButton.Dock = DockStyle.Fill;
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
            ResumeLayout();
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
            Font = new Font(FontContainer.Instance.Families[0], 15);
            title.Font = ConvertToDesiredHeight(Font, title.Height);
            createNoteButton.Font = Font;
        }

        private void Setup_SaveLoadButtons()
        {
            var getIcoRect = (Button bt) => new Rectangle(0, 3, bt.Height - 6, bt.Height - 6);
            saveButton.Paint += (sender, e) =>
                e.Graphics.DrawImage(Resources.DownloadIcon, getIcoRect((Button)sender!));
            saveButton.Click += (sender, e) =>
            {
                FileManager.Instance.WriteToDataFile(NotesContainer);
            };

            loadButton.Paint += (sender, e) =>
                e.Graphics.DrawImage(Resources.UploadIcon, getIcoRect((Button)sender!));
            loadButton.Click += (sender, e) =>
            {
                Debug.WriteLine(FileManager.Instance.RecreateNotesFromData());
            };
            saveButton.BackColor = sortStripButton.BackColor;
            saveButton.FlatStyle = FlatStyle.Flat;
            loadButton.BackColor = sortStripButton.BackColor;
            loadButton.FlatStyle = FlatStyle.Flat;
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

        private void SetupLayout()
        {
            var mainColor = Color.FromArgb(0x768DE2);
            LabelBackColor = ControlPaint.LightLight(Color.FromArgb(255, mainColor));
            panelLabel.BackColor = LabelBackColor;
            mainLayoutTable.BackColor = Color.White;
            ContentBackColor = ControlPaint.Light(Color.FromArgb(255, mainColor), 1.5f);
            contentPanel.BackColor = ContentBackColor;
            contentPanel.AutoScroll = true;
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

            Setup_ReassignFonts();
        }
    }
}
