using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models
{
    public class NoteControlMidder<ParentNoteType,InnerNotesType> : NoteControlParent<InnerNotesType>,INoteControlChild<ParentNoteType>,INoteControlParent<InnerNotesType>,INoteMidder<ParentNoteType,InnerNotesType> where ParentNoteType:NoteControl where InnerNotesType : NoteControl
    {
        public ParentNoteType ParentNote { get; set; }
        ParentNoteType INoteChild<ParentNoteType>.ParentNote => ParentNote;
        protected NoteControlMidder(ParentNoteType parent, Color stripColor, string text, string description) : base(stripColor, text, description)
        {
            ParentNote = parent;
        }
    }

}
