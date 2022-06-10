using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models
{
    public class NoteControlChild<ParentNoteType> : NoteControl, INoteChild<ParentNoteType> where ParentNoteType : NoteControl
    {
        public ParentNoteType ParentNote { get; set; }
        ParentNoteType INoteChild<ParentNoteType>.ParentNote => ParentNote;
        protected NoteControlChild(ParentNoteType parent,Color stripColor, string text, string description) : base(stripColor, text, description)
        {
            ParentNote = parent;
        } 
    }

}
