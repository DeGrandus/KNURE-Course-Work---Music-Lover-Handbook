using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlMidder : NoteControlParent, INoteControlChild, INoteChild
    {
        public INoteControlParent ParentNote { get; set; }

        INoteParent INoteChild.ParentNote => (INoteParent)ParentNote;

        protected NoteControlMidder(INoteControlParent parent, string text, string description)
            : base(text, description)
        {
            ParentNote = parent;
        }

        public override void SetupColorTheme(NoteType type)
        {
            ThemeColor =
                type.GetColor()
                ?? ParentNote.InnerNotes.LastOrDefault()?.ThemeColor
                ?? Color.Transparent;
        }

        public override void UpdateSize()
        {
            base.UpdateSize();
            ParentNote?.UpdateSize();
        }
    }
}
