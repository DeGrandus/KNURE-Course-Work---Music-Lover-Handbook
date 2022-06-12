using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteAuthor : NoteControlParent
    {
        public override NoteType Type => NoteType.Author;

        public NoteAuthor(string text, string description) : base(text, description)
        {
            InitializeComponent();
        }
    }
}
