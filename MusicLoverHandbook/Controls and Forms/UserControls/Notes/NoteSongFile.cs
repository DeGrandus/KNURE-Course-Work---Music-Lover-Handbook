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
    public partial class NoteSongFile : NoteControlChild<NoteSong>
    {
        public override NoteType Type { get; } = NoteType.SongFile;

        public NoteSongFile(NoteSong song, string text, string description) : base(song, text, description)
        {
            InitializeComponent();
        }
    }
}
