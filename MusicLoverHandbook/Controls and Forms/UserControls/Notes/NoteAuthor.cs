using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteAuthor : NoteControlMidder
    {
        public override NoteType NoteType => NoteType.Author;

        public NoteAuthor(NoteDisc disc, string text, string description)
            : base(disc, text, description)
        {
            InitializeComponent();
        }
    }
}
