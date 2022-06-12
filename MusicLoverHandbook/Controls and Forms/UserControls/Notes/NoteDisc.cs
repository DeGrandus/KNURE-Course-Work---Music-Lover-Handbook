using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteDisc : NoteControlMidder
    {
        public override NoteType NoteType { get; } = NoteType.Disc;

        public NoteDisc(NoteAuthor author, string text, string description)
            : base(author, text, description)
        {
            InitializeComponent();
        }
    }
}
