using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MusicLoverHandbook.Models.Inerfaces.IControlTheme;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteContainer : UserControl
    {
        public MainForm MainForm { get; }
        public Panel NotesPanel { get; }
        public NoteContainer(MainForm form) { 
            InitializeComponent();
            MainForm = form;

            NotesPanel = new Panel() { Dock = DockStyle.Fill,AutoScroll=true};
            Controls.Add(NotesPanel);
            
        }      
    }
}
