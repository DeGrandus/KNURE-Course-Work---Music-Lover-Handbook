using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlMidder : NoteControlParent, INoteControlChild, INoteChild
    {
        public IParentControl ParentNote { get; set; }

        INoteParent INoteChild.ParentNote => (INoteParent)ParentNote;

        protected NoteControlMidder(
            IParentControl parent,
            string text,
            string description,
            NoteType noteType,
            NoteCreationOrder? order
        ) : base(text, description, noteType, order)
        {
            ParentNote = parent;
        }

        public bool ContainsInParentTree(IContainerControl potentialParent)
        {
            if (potentialParent == ParentNote)
                return true;
            if (ParentNote is INoteControlChild asChild)
                return asChild.ContainsInParentTree(potentialParent);
            return false;
        }

        public INoteControlParent? GetFirstNoteControlParent()
        {
            return ParentNote is INoteControlParent parent
              ? parent is INoteControlChild child
                  ? child.GetFirstNoteControlParent()
                  : parent
              : null;
        }

        public IParentControl GetFirstParent()
        {
            return ParentNote is INoteControlChild child ? child.GetFirstParent() : ParentNote;
        }

        public override void SetupColorTheme(NoteType type)
        {
            ThemeColor =
                type.GetColor()
                ?? (
                    ParentNote is IParentControl asParent
                        ? asParent.InnerNotes.LastOrDefault()?.ThemeColor
                        : null
                )
                ?? Color.Transparent;
        }

        public override void UpdateSize()
        {
            base.UpdateSize();
            if (ParentNote is INoteControlParent noteParent)
                noteParent.UpdateSize();
        }
    }
}
