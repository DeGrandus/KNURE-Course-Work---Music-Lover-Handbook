using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using System.ComponentModel;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteSong
        : NoteControlMidder
    {
        public override NoteType Type { get; } = NoteType.Song;
        public NoteSong(NoteControlParent parent, string text, string description)
            : base(parent, text, description)
        {
            InitializeComponent();
        }
    }
}
