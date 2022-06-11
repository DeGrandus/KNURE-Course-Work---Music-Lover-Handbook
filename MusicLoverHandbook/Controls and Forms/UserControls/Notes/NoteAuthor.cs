using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteAuthor : NoteControlParent<INoteControl>
    {
        public override NoteType Type => NoteType.Author;

        public NoteAuthor(string text, string description) : base(text, description)
        {
            InitializeComponent();
        }
    }
}
