using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteSong : NoteControlMidder
    {
        public override NoteType NoteType { get; } = NoteType.Song;
        public override NoteCreationOrder? UsedCreationOrder { get; }
        public NoteSong(NoteControlParent parent, string text, string description, NoteCreationOrder? usedCreationOrder)
            : base(parent, text, description)
        {
            InitializeComponent();
            Icon = Properties.Resources.song;
            UsedCreationOrder = usedCreationOrder;
        }
    }
}
