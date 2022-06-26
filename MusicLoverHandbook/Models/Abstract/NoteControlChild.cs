using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlChild : NoteControl, INoteChild, INoteControlChild
    {
        private DelayedSetup? delayedSetup;
        private bool inited = false;

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

        private delegate void DelayedSetup();

        public IParentControl ParentNote { get; set; }

        INoteParent INoteChild.ParentNote => (INoteParent)ParentNote;

        public override void SetupColorTheme(NoteType type)
        {
            var themeColor = () => type.GetColor() ?? Color.Transparent;
            if (inited)
                ThemeColor = themeColor();
            else
                delayedSetup += () =>
                {
                    ThemeColor = themeColor();
                };
        }

        protected override void InitCustomLayout()
        {
            if (inited)
                base.InitCustomLayout();
            else
                delayedSetup += base.InitCustomLayout;
        }

        protected override void InitValues(string text, string description)
        {
            if (inited)
                base.InitValues(text, description);
            else
                delayedSetup += () => base.InitValues(text, description);
        }

        public IParentControl GetFirstParent()
        {
            return ParentNote is INoteControlChild child ? child.GetFirstParent() : ParentNote;
        }

        public INoteControlParent? GetFirstNoteControlParent()
        {
            return ParentNote is INoteControlParent parent
              ? parent is INoteControlChild child
                  ? child.GetFirstNoteControlParent()
                  : parent
              : null;
        }

        public bool ContainsInParentTree(IContainerControl potentialParent)
        {
            if (potentialParent == ParentNote)
                return true;
            if (ParentNote is INoteControlChild asChild)
                return asChild.ContainsInParentTree(potentialParent);
            return false;
        }
    }
}
