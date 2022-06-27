using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteSong : NoteControlMidder
    {
        public NoteSong(
            IParentControl parent,
            string text,
            string description,
            NoteCreationOrder? usedCreationOrder
        ) : base(parent, text, description, NoteType.Song, usedCreationOrder)
        {
            InitializeComponent();
            Icon = Properties.Resources.SongIcon;
        }
    }
}