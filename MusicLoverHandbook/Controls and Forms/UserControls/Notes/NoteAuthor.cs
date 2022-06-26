using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteAuthor : NoteControlMidder
    {
        public NoteAuthor(
            IParentControl disc,
            string text,
            string description,
            NoteCreationOrder? usedOrder
        ) : base(disc, text, description, NoteType.Author, usedOrder)
        {
            InitializeComponent();
            Icon = Properties.Resources.author;
        }
    }
}
