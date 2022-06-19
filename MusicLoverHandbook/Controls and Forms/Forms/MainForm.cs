using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Logic.Notes;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.View.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace MusicLoverHandbook.Controls_and_Forms.Forms
{
    public partial class MainForm : Form
    {
        private float rtitle,
            rcont,
            radd,
            rdrag;

        public Color LabelBackColor;
        public Color ContentBackColor;
        public NotesContainer Container { get; }

        public MainForm()
        {
            InitializeComponent();

            MinimumSize = new Size(300, 565);

            var rowSt = tableLayoutPanel1.RowStyles;
            rtitle = rowSt[0].Height;
            rcont = rowSt[2].Height;
            radd = rowSt[4].Height;
            rdrag = rowSt[6].Height;

            Container = new NotesContainer(panelContent);

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
            tableLayoutPanel1.BackColor = Color.White;

            panelContent.AutoScroll = true;
            ContentBackColor = ControlPaint.Light(
                Color.FromArgb(255, Color.FromArgb(0x768DE2)),
                1.5f
            );
            panelContent.BackColor = ContentBackColor;
            dragInto.BackColor = panelLabel.BackColor;
            Debug.WriteLine("");

            Debug.WriteLine(dragInto.BackColor);
            Debug.WriteLine(panelLabel.BackColor);
            createNoteButton.FlatAppearance.BorderSize = 2;

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
                BuildDragImage();
            };
            Load += (sender, e) =>
            {
                AdaptToSize();
                BuildDragImage();
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

        private void BuildDragImage()
        {
            var image = new Bitmap(dragInto.Width, dragInto.Height);
            var text = "Drop .mp3 file here to fast load into base";
            var g = Graphics.FromImage(image);
            g.Clear(dragInto.BackColor);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            using (var pen = new Pen(ControlPaint.Dark(dragInto.BackColor)))
            using (var textbrush = new Pen(Color.Black).Brush)
            {
                pen.Width = 20;
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                pen.DashPattern = new[] { 2f, 1 };
                g.DrawRectangle(pen, new(new(0, 0), image.Size));
                var font = GetScaledFontWidthUpscaled();
                font = new Font(font.FontFamily, font.SizeInPoints - 3, GraphicsUnit.Point);
                var strMeasure = g.MeasureString(text, font);
                var pt = (Point)(
                    dragInto.Size / 2
                    - new Size((int)strMeasure.Width / 2, (int)strMeasure.Height / 2)
                );
                g.DrawString(text, font, textbrush, pt);
            }
            dragInto.BackgroundImage = image;
        }

        private void AdaptToSize()
        {
            if (Size.Width < MinimumSize.Width || Size.Height < MinimumSize.Height)
                return;
            SuspendLayout();
            var colSt = tableLayoutPanel1.ColumnStyles;
            var rowSt = tableLayoutPanel1.RowStyles;

            var cols = tableLayoutPanel1.ColumnCount;
            var rows = tableLayoutPanel1.RowCount;

            var border = Width > 400 ? 15 : (((float)Width - 300) / 100 * 15);
            var addbt = Width > 800 ? 100 : (((float)Width - 300) / 500 * 100);
            var dropfile = Width > 1000 ? 150 : (((float)Width - 300) / 700 * 150);

            colSt[0].Width = border;
            colSt[cols - 1].Width = border;
            colSt[1].Width = addbt;
            colSt[cols - 2].Width = addbt;
            colSt[2].Width = dropfile;
            colSt[cols - 3].Width = dropfile;

            var sum = new[] { rtitle, rcont, radd, rdrag }.Aggregate((c, x) => c + x);
            var onlyAdd = Height - 40 < sum;
            var fixcont = Height + 40 < sum;
            if (Height > 800 && Height < 2000)
            {
                var hdiff = ((float)Height - 800) / 1200;

                rowSt[2].Height = rcont + 800 * hdiff;
            }
            if (Height < 800)
            {
                rowSt[6].Height =
                    rdrag * ((float)Height - MinimumSize.Height) / (800 - MinimumSize.Height);
                rowSt[5].Height = 0;
                rowSt[0].Height = 0;
                rowSt[1].SizeType = SizeType.Absolute;
                rowSt[1].Height = 0;
                rowSt[3].Height = 0;
            }
            else
            {
                rowSt[0].Height = 50;
                rowSt[1].SizeType = SizeType.Percent;
                rowSt[1].Height = 25;
                rowSt[3].Height = 25;
                rowSt[5].Height = 25;
            }

            ResumeLayout();
        }
    }
}
