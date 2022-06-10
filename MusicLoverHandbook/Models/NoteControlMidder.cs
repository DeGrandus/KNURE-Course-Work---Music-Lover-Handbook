using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models
{
    public abstract class NoteControlMidder<ParentNoteType, InnerNotesType>
        : NoteControlParent<InnerNotesType>,
          INoteControlMidder<ParentNoteType, InnerNotesType>
        where ParentNoteType : INoteControl
        where InnerNotesType : INoteControl
    {
        public ParentNoteType ParentNote { get; set; }
        ParentNoteType INoteChild<ParentNoteType>.ParentNote => ParentNote;

        protected NoteControlMidder(
            ParentNoteType parent,
            string text,
            string description
        ) : base(text, description)
        {
            ParentNote = parent;
        }
    }
}
