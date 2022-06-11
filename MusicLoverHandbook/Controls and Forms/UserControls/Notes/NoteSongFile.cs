using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteSongFile : NoteControlChild<NoteSong>
    {
        public override NoteType Type { get; } = NoteType.SongFile;

        public NoteSongFile(NoteSong song, string text, string description)
            : base(song, text, description)
        {
            InitializeComponent();
        }
    }
}
