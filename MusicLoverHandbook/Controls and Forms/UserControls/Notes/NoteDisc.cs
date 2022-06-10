using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteDisc : NoteControlMidder<NoteAuthor, NoteSong>
    {
        public override NoteType Type { get; } = NoteType.Disc;
        public NoteDisc(NoteAuthor author, string text,string description) : base(author, text, description)
        {
            InitializeComponent();
        }
    }
}
