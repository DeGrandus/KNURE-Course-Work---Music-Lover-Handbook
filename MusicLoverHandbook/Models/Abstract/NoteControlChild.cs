using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Inerfaces;
using System.Diagnostics;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlChild : NoteControl, INoteControlChild
    {
        #region Private Fields

        private DelayedSetup? delayedSetup;
        private bool inited = false;

        #endregion Private Fields

        #region Public Properties

        public IParentControl ParentNote { get; set; }

        INoteParent INoteChild.ParentNote => ParentNote as INoteParent ?? null;

        #endregion Public Properties

        #region Protected Constructors

        protected NoteControlChild(
            IParentControl parent,
            string text,
            string description,
            NoteType noteType,
            NoteCreationOrder? order
        ) : base(text, description, noteType, order)
        {
            ParentNote = parent;
            if (delayedSetup != null)
                delayedSetup();
            inited = true;
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
            Debug.WriteLine(GetType());
            Debug.WriteLine(ParentNote);
            return ParentNote is INoteControlChild child ? child.GetFirstParent() : ParentNote;
        }

        public override void SetupColorTheme(NoteType type)
        {
            var themeColor = () => type.GetColor() ?? Color.Transparent;
            if (inited)
                MainColor = themeColor();
            else
                delayedSetup += () =>
                {
                    MainColor = themeColor();
                };
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void InitValues(string text, string description)
        {
            if (inited)
                base.InitValues(text, description);
            else
                delayedSetup += () => base.InitValues(text, description);
        }

        protected override void SetupLayout()
        {
            if (inited)
                base.SetupLayout();
            else
                delayedSetup += base.SetupLayout;
        }

        #endregion Protected Methods

        #region Private Delegates

        private delegate void DelayedSetup();

        #endregion Private Delegates
    }
}
