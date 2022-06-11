using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteDisc : NoteControlMidder<INoteControlParent<INoteControl>, INoteControl>
    {
        public override NoteType Type { get; } = NoteType.Disc;

        public NoteDisc(NoteAuthor author, string text, string description)
            : base(author, text, description)
        {
            InitializeComponent();
        }
    }
}
