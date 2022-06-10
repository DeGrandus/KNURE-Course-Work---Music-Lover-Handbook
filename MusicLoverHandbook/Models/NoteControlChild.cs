using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models
{
    public abstract class NoteControlChild<ParentNoteType> : NoteControl, INoteChild<ParentNoteType>, INoteControlChild<ParentNoteType> where ParentNoteType : INoteControl
    {
        public ParentNoteType ParentNote { get; set; }
        ParentNoteType INoteChild<ParentNoteType>.ParentNote => ParentNote;
        protected NoteControlChild(ParentNoteType parent, string text, string description) : base(text, description)
        {
            ParentNote = parent;
        }
    }

}
