using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Inerfaces;
using System.ComponentModel;
using System.Diagnostics;

namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    public partial class MainForm : Form
    {
        public Color LabelBackColor;
        public Color ContentBackColor;

        public NotesContainer NotesContainer { get; }
        public MainForm()
        {
            InitializeComponent();

            MinimumSize = new Size(300, 565);

            NotesContainer = new NotesContainer(contentPanel,qSTextBox,qSSwitchLabel);

            SetupLayout();
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

        private void SetupLayout()
        {
            LabelBackColor = ControlPaint.LightLight(Color.FromArgb(255, Color.FromArgb(0x768DE2)));
            panelLabel.BackColor = LabelBackColor;
            mainLayoutTable.BackColor = Color.White;

            contentPanel.AutoScroll = true;
            ContentBackColor = ControlPaint.Light(
                Color.FromArgb(255, Color.FromArgb(0x768DE2)),
                1.5f
            );
            contentPanel.BackColor = ContentBackColor;
            searchBarLayout.BackColor = LabelBackColor;

            createNoteButton.FlatAppearance.BorderSize = 2;

            qSSwitchLabel.BasicTooltipText = "[ Match only names ]";
            qSSwitchLabel.SpecialTooltipText = "[ Match both names and descriptions ]";
            qSSwitchLabel.BackColorChanged += (sender, e) =>
            {
                qSPanel.BackColor = qSSwitchLabel.BackColor;
            };

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

            createNoteButton.Click += (sender, e) =>
            {
                var controller = new NoteCreationMenuController(this);

                var creationResult = controller.OpenCreationMenu();
                creationResult?.CreateNote();
            };

            ReassignFonts();
        }

        private void ReassignFonts()
        {
            title.Font = ConvertToDesiredHeight(GetScaledFontWidthUpscaled(), title.Height);
            createNoteButton.Font = GetScaledFontWidthUpscaled();
        }
        private Font GetScaledFontWidthUpscaled()
        {
            var fontfam = FontContainer.Instance.Families[0];
            var font = new Font(fontfam, 12);
            var pts =
                (MinimumSize.Width)
                * 12
                / (Graphics.FromHwnd(Handle).MeasureString(createNoteButton.Text, font).Width + 30);
            return new Font(font.FontFamily, (float)pts);
        }

        private Font ConvertToDesiredHeight(Font font, int h)
        {
            return new Font(font.FontFamily, h, font.Style, GraphicsUnit.Pixel);
        }

        private void AdaptToSize()
        {
            if (Size.Width < MinimumSize.Width || Size.Height < MinimumSize.Height)
                return;
            int rmax = mainLayoutTable.RowCount - 1, cmax = mainLayoutTable.ColumnCount - 1;
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

            tb.RowStyles[1].Height = (hDiff <= hLim * 2 ? hDiff <= hLim ? 0 : (((float)hDiff - 100) / hLim) : 1) * 50;
        }
    }
}
