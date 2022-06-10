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
    public partial class NoteAuthor : NoteControlMidder<INoteControl, INoteControlChild<INoteControl>>
    {
        public override NoteType Type => NoteType.Author;

        public NoteAuthor(NoteContainer container, string text, string description) : base(container, text, description)
        {
            InitializeComponent();
        }

    }
}
