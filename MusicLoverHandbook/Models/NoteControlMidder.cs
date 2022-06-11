using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models
{
    public abstract class NoteControlMidder<ParentNoteType, InnerNotesType> :
        NoteControlParent<InnerNotesType>,
        INoteControlChild<ParentNoteType>,
        INoteChild<ParentNoteType>
        where ParentNoteType : INoteControlParent<INoteControl>
        where InnerNotesType : INoteControl
    {
        public ParentNoteType ParentNote { get; set; }

        protected NoteControlMidder(ParentNoteType parent, string text, string description)
            : base(text, description)
        {
            ParentNote = parent;
        }
        protected override void SetupColorTheme(NoteType type)
        {
            ThemeColor = type.GetColor() ?? (ParentNote as IControlTheme)?.ThemeColor ?? Color.Transparent;
        }
        public override void UpdateSize()
        {
            base.UpdateSize();
            ParentNote?.UpdateSize();
        }
    }
}
