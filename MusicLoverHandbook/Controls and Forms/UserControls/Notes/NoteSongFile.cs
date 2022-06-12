using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteSongFile : NoteControlChild
    {
        public override NoteType NoteType { get; } = NoteType.SongFile;

        public NoteSongFile(NoteSong song, string text, string description)
            : base(song, text, description)
        {
            InitializeComponent();
        }
    }
}
