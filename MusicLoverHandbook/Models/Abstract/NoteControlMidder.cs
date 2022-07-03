using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Inerfaces;
using System.Diagnostics;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlMidder : NoteControlParent, INoteControlChild
    {
        #region Public Properties

        public IParentControl ParentNote { get; set; }

        INoteParent INoteChild.ParentNote => ParentNote as INoteParent ?? null;

        #endregion Public Properties

        #region Protected Constructors

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

        #endregion Protected Constructors

        #region Public Methods

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
            Debug.WriteLine("MIDDER: " + GetType());
            Debug.WriteLine(this.ParentNote);
            return ParentNote is INoteControlChild child ? child.GetFirstParent() : ParentNote;
        }

        public override void SetupColorTheme(NoteType type)
        {
            MainColor =
                type.GetColor()
                ?? (
                    ParentNote is IParentControl asParent
                        ? asParent.InnerNotes.LastOrDefault()?.MainColor
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

        #endregion Public Methods
    }
}
