using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Diagnostics;

namespace MusicLoverHandbook.Models
{
    public abstract class NoteControlChild<ParentNoteType>
        : NoteControl,
          INoteChild<ParentNoteType>,
          INoteControlChild<ParentNoteType> where ParentNoteType : INoteControlParent<INoteControl>
    {
        public ParentNoteType ParentNote { get; set; }
        ParentNoteType INoteChild<ParentNoteType>.ParentNote => ParentNote;

        protected NoteControlChild(ParentNoteType parent, string text, string description)
            : base(text, description)
        {
            ParentNote = parent;
        }
        protected override void SetupColorTheme(NoteType type)
        {
            Debug.WriteLine(ParentNote is INoteControl);
            ThemeColor = type.GetColor() ?? (ParentNote as IControlTheme)?.ThemeColor ?? Color.Transparent;
        }
    }
}
