using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlMidder : NoteControlParent, INoteControlChild, INoteChild
    {
        public IControlParent ParentNote { get; set; }
        INoteParent INoteChild.ParentNote => (INoteParent)ParentNote;

        protected NoteControlMidder(IControlParent parent, string text, string description)
            : base(text, description)
        {
            ParentNote = parent;
        }

        public override void SetupColorTheme(NoteType type)
        {
            ThemeColor =
                type.GetColor()
                ?? (ParentNote is IControlParent asParent ? asParent.InnerNotes.LastOrDefault()?.ThemeColor : null)
                ?? Color.Transparent;
        }
        public override void UpdateSize()
        {
            base.UpdateSize();
            if (ParentNote is INoteControlParent noteParent)noteParent.UpdateSize();
        }
    }
}
