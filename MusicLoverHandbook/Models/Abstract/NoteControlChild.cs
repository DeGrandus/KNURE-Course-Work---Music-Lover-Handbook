using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Diagnostics;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlChild
        : NoteControl,
          INoteChild,
          INoteControlChild
    {
        public INoteControlParent ParentNote { get; set; }
        INoteParent INoteChild.ParentNote => (INoteParent)ParentNote;

        private delegate void DelayedSetup();
        private DelayedSetup delayedSetup;
        protected NoteControlChild(INoteControlParent parent, string text, string description)
            : base(text, description)
        {
            ParentNote = parent;
            if (delayedSetup != null) delayedSetup();
        }
        public override void SetupColorTheme(NoteType type)
        {
            delayedSetup += () =>
            {
                Debug.WriteLine(ParentNote.InnerNotes.LastOrDefault());
                ThemeColor = type.GetColor() ?? ParentNote.InnerNotes.LastOrDefault()?.ThemeColor ?? Color.Transparent;
            };
        }
        protected override void ConstructLayout()
        {
            delayedSetup += base.ConstructLayout;
        }
    }
}
