using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteAdd : NoteControlChild
    {
        public override NoteType NoteType { get; } = NoteType.AddButton;

        public NoteAdd(NoteControlParent parent, string noteText, string noteDescription)
            : base(parent, noteText, noteDescription)
        {
            InitializeComponent();
            SideButtons.Deactivate(ButtonType.Delete);
            SideButtons.Deactivate(ButtonType.Edit);
            Icon = Properties.Resources.add;
        }
    }
}
