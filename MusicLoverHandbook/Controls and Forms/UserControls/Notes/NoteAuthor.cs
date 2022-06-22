using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteAuthor : NoteControlMidder
    {
        public override NoteType NoteType => NoteType.Author;
        public override NoteCreationOrder? UsedCreationOrder { get; }
        public NoteAuthor(IControlParent disc, string text, string description, NoteCreationOrder? usedOrder)
            : base(disc, text, description)
        {
            InitializeComponent();
            UsedCreationOrder = usedOrder;
            Icon = Properties.Resources.author;
        }
    }
}
