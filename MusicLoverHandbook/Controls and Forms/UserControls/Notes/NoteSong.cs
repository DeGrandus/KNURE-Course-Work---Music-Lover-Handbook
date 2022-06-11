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
    public partial class NoteSong : NoteControlMidder<INoteControlParent<INoteControl>, NoteSongFile>
    {
        public override NoteType Type { get; } = NoteType.Song;

        [DesignOnly(true)]
        public NoteSong() : this(null,null,null)
        {

        }
        public NoteSong(INoteControlParent<INoteControl> parent,string text, string description) : base(parent, text, description)
        {
            InitializeComponent();
        }
    }
}
