using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteAdd : NoteControlChild<NoteControlParent<NoteControl>>
    {
        public override NoteType Type { get; } = NoteType.AddButton;

        public NoteAdd(
            NoteControlParent<NoteControl> parent,
            string noteText,
            string noteDescription
        ) : base(parent, noteText, noteDescription)
        {
            InitializeComponent();
        }
    }
}
