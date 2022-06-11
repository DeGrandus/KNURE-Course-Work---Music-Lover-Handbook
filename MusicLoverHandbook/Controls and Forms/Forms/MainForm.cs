using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            MinimumSize = new Size(300, 555);

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

            Resize += (sender, e) =>
            {
                DoResizes();
            };
            Load += (sender, e) => DoResizes();
        }

        private void DoResizes()
        {
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
        }
    }
}
