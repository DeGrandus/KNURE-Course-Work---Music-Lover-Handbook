using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteAdd : NoteControlChild<INoteControlParent<INoteControl>>
    {
        public override NoteType Type { get; } = NoteType.AddButton;

        public NoteAdd(
            INoteControlParent<INoteControl> parent,
            string noteText,
            string noteDescription
        ) : base(parent, noteText, noteDescription)
        {
            InitializeComponent();
        }
    }
}
