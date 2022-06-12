using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Diagnostics;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlChild : NoteControl, INoteChild, INoteControlChild
    {
        private bool inited = false;
        public INoteControlParent ParentNote { get; set; }

        INoteParent INoteChild.ParentNote => (INoteParent)ParentNote;

        private delegate void DelayedSetup();
        private DelayedSetup delayedSetup;

        protected NoteControlChild(INoteControlParent parent, string text, string description)
            : base(text, description)
        {
            ParentNote = parent;
            if (delayedSetup != null)
                delayedSetup();
            inited = true;
        }

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

        protected override void ConstructLayout()
        {
            if (inited)
                base.ConstructLayout();
            else
                delayedSetup += base.ConstructLayout;
        }
    }
}
