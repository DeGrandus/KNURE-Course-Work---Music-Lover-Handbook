using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Logic.Notes;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Inerfaces;
using System.ComponentModel;
using System.Diagnostics;

namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    public partial class MainForm : Form
    {
        public Color ContentBackColor;
        public Color LabelBackColor;
        public NoteBuilder Builder { get; }
        public NoteManager NoteManager { get; }

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            MinimumSize = new Size(300, 565);
            Builder = new(this);
            NoteManager = new(this);
            NotesContainer = new NotesContainer(contentPanel, qSTextBox, qSSwitchLabel);

            SetupLayout();
        }

        public NotesContainer NotesContainer { get; }

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


        private void Setup_ReassignFonts()
        {
            Font = new Font(FontContainer.Instance.Families[0], 15);
            title.Font = ConvertToDesiredHeight(Font,title.Height);
            createNoteButton.Font = Font;
        }

        private void SetupLayout()
        {
            LabelBackColor = ControlPaint.LightLight(Color.FromArgb(255, Color.FromArgb(0x768DE2)));
            panelLabel.BackColor = LabelBackColor;
            mainLayoutTable.BackColor = Color.White;
            ContentBackColor = ControlPaint.Light(
                Color.FromArgb(255, Color.FromArgb(0x768DE2)),
                1.5f
            );
            contentPanel.BackColor = ContentBackColor;
            contentPanel.AutoScroll = true;
            searchBarLayout.BackColor = LabelBackColor;

            Setup_CreateNoteButton_Base();
            Setup_CreateNoteButton_AnimationWorker();

            Setup_SortingStripButton_Base();
            Setup_SortingStripButton_MenuStrip();

            Setup_QSSwitch();

            Setup_AdvancedSearchButton();
            
            Setup_ReassignFonts();
        }

        private void Setup_SortingStripButton_MenuStrip()
        {
            var strip = new ContextMenuStrip()
            {
            };
            var alphabeticalSortRadio = new RadioButton()
            {
                Text = "Alphabeticaly",
                Tag = (IEnumerable<INoteControlChild> children) => children.OrderBy(x => x.NoteName)
            };
            var contentSortRadio = new RadioButton()
            {
                Text = "By content amount",
                Tag = (IEnumerable<INoteControlChild> children) => children.OrderBy(x => x is IParentControl p ? p.InnerNotes.Count : 0)
            };
            var radioGroup = new[] { alphabeticalSortRadio, contentSortRadio }.ToList();
            var reversiveSortCheck = new CheckBox()
            {
                Text = "Reverse",
                Tag = (IEnumerable<INoteControlChild> children) => children.Reverse()
            };
            var applyFilteringButton = new ToolStripButton()
            {
                Text = "Apply sorting"
            };
            var clearFilteringButton = new ToolStripButton()
            {
                Text = "Clear filters"
            };
            applyFilteringButton.Click += (sender, e) =>
            {
                var filter = radioGroup.FirstOrDefault(x => x.Checked)?.Tag;
                var reverse = reversiveSortCheck.Checked ? reversiveSortCheck.Tag : null;
                var filtersConverted = new List<object?>() { filter, reverse }.OfType<Func<IEnumerable<INoteControlChild>, IEnumerable<INoteControlChild>>>();
                NotesContainer.Filters = filtersConverted.ToList();
            };
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

        private void Setup_AdvancedSearchButton()
        {
            advFilterButton.BackColor = ControlPaint.Light(advFilterButton.Parent.BackColor);
            advFilterButton.FlatAppearance.BorderColor = ControlPaint.Dark(
                advFilterButton.Parent.BackColor,
                0.2f
            );
            advFilterButton.FlatAppearance.BorderSize = 1;
            advFilterButton.Click += (sender, e) =>
            {
                var filterMenu = new NoteAdvancedFilterMenu(this).ShowDialog();
                if (filterMenu == DialogResult.OK)
                    return;
            };
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

        private void Setup_CreateNoteButton_Base()
        {
            createNoteButton.FlatAppearance.BorderSize = 2;
            createNoteButton.Text = "Create new Note";
            createNoteButton.Click += (sender, e) =>
            {
                var controller = new NoteCreationMenuController(this);

                var creationResult = controller.OpenCreationMenu();
                SuspendLayout();
                creationResult?.CreateNote();
                NotesContainer.InvokeQuickSearch();
                ResumeLayout();
            };
        }
    }
    public class CustomProfessionalColorTable : ProfessionalColorTable
    {
        public CustomProfessionalColorTable(Color imageStripColor)
        {
            this.imageStripColor = imageStripColor;
        }
        private Color imageStripColor;
        public override Color ImageMarginGradientBegin => imageStripColor;
        public override Color ImageMarginGradientEnd => imageStripColor;
        public override Color ImageMarginRevealedGradientMiddle => imageStripColor;
    }
    public class ColoredIconsBarToolStripRenderer : ToolStripProfessionalRenderer
    {
        public ColoredIconsBarToolStripRenderer(Color color) : base(new CustomProfessionalColorTable(color)) { }
    }
}
