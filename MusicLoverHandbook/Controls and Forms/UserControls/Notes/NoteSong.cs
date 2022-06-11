using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;
using System.ComponentModel;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteSong
        : NoteControlMidder<INoteControlParent<INoteControl>, NoteSongFile>
    {
        public override NoteType Type { get; } = NoteType.Song;

        [DesignOnly(true)]
        public NoteSong() : this(null, null, null) { }

        public NoteSong(INoteControlParent<INoteControl> parent, string text, string description)
            : base(parent, text, description)
        {
            InitializeComponent();
        }
    }
}
