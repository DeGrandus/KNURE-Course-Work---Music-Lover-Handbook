using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models;
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

        public MainForm()
        {
            InitializeComponent();

            MinimumSize = new Size(300, 565);

            var note = new NoteAuthor("Єбать мене в рот", "test");
            var noteIn = new NoteSong(note, "Ніхуя собі!", "test");
            var noteIn2 = new NoteSongFile(noteIn, "Да???", "test");
            var noteIn22 = new NoteSongFile(noteIn, "ПІЗДА!!!", "test");
            var note2 = new NoteSong(null, "Воно працює!", "test");

            var rowSt = tableLayoutPanel1.RowStyles;
            rtitle = rowSt[0].Height;
            rcont = rowSt[2].Height;
            radd = rowSt[4].Height;
            rdrag = rowSt[6].Height;

            SetupLayout();
        }

        private Color[] CreateGradient()
        {
            var step = 10;
            var colors = new[] { Color.Red, Color.Orange, Color.Gold, Color.Green, Color.LightBlue, Color.DarkBlue, Color.BlueViolet, Color.Red };
            var groups = new List<(Color from, Color to)>();
            colors.Aggregate((c, x) => { groups.Add((c, x)); return x; });

            var gradiented = new List<Color>();
            groups.ForEach(x =>
            {
                int fR = x.from.R, fG = x.from.G, fB = x.from.B;
                int tR = x.to.R, tG = x.to.G, tB = x.to.B;

                int sR = (tR - fR);
                int sG = (tG - fG);
                int sB = (tB - fB);

                for (double i = 0; i < 1; i += 1.0 / step)
                {
                    gradiented.Add(Color.FromArgb(255, (int)Math.Round(fR + sR * i), (int)Math.Round(fG + sG * i), (int)Math.Round(fB + sB * i)));
                }
            });
            return gradiented.ToArray();
        }
        private void SetupLayout()
        {

            dragInto.BackColor = panelLabel.BackColor;

            var worker = new BackgroundWorker();
            createNoteButton.FlatAppearance.BorderSize = 2;
            worker.DoWork += (sender, e) =>
            {
                var colors = CreateGradient();
                var ind = 0;
                while (true)
                {
                    if (ind + 1 >= colors.Length) ind = 1;
                    createNoteButton.FlatAppearance.BorderColor = colors[ind];
                    createNoteButton.FlatAppearance.MouseDownBackColor = ControlPaint.LightLight(ControlPaint.Light(colors[ind], 1f));
                    createNoteButton.BackColor = ControlPaint.LightLight(ControlPaint.LightLight(ControlPaint.LightLight(colors[ind])));
                    ind++;
                    Thread.Sleep(100);
                }
            };
            worker.RunWorkerAsync();

            ResetFonts();

            Resize += (sender, e) =>
            {
                DoResizes();
                CreateDragNDropImage();
            };
            Load += (sender, e) => { DoResizes(); CreateDragNDropImage(); };

        }
        private void ResetFonts()
        {
            
            createNoteButton.Font = GetScaledFont();
        }
        private Font GetScaledFont()
        {
            var fontfam = FontContainer.Instance.Families[0];
            var font = new Font(fontfam, 12);
            var pts = (MinimumSize.Width) * 12 / (Graphics.FromHwnd(Handle).MeasureString(createNoteButton.Text, font).Width + 30);
            return new Font(font.FontFamily, (float)pts);
        }
        private void CreateDragNDropImage()
        {
            var image = new Bitmap(dragInto.Width, dragInto.Height);
            var text = "Drop .mp3 file here";
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
                var font = GetScaledFont();
                font = new Font(font.FontFamily, font.SizeInPoints-3,GraphicsUnit.Point);
                var strMeasure = g.MeasureString(text, font);
                var pt = (Point)(dragInto.Size / 2 - new Size((int)strMeasure.Width / 2, (int)strMeasure.Height / 2));
                g.DrawString(text, font, textbrush, pt);
            }
            dragInto.BackgroundImage = image;
        }
        private void DoResizes()
        {
            if (Size.Width < MinimumSize.Width || Size.Height < MinimumSize.Height) return;
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

            if (dragInto.Height < 20)
            {
                
                dragInto.Visible = true;

            }
            else
            {
                dragInto.Visible = false;

            }

            ResumeLayout();

        }
    }
}
