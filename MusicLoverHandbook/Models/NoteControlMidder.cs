using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models
{
    public class NoteControlMidder<ParentNoteType,InnerNotesType> : NoteControl,INoteMidder<ParentNoteType,InnerNotesType> where ParentNoteType:NoteControl where InnerNotesType : NoteControl
    {
        public ParentNoteType ParentNote { get; set; }
        ParentNoteType INoteChild<ParentNoteType>.ParentNote => ParentNote;
        public ObservableCollection<InnerNotesType> InnerNotes { get; set; } = new();
        IReadOnlyCollection<InnerNotesType> INoteParent<InnerNotesType>.InnerNotes => InnerNotes.ToList();

        public ContentLinker<InnerNotesType> Linker { get; }

        protected NoteControlMidder(ParentNoteType parent, Color stripColor, string text, string description) : base(stripColor, text, description)
        {
            Linker = new ContentLinker<InnerNotesType>(this);
            ParentNote = parent;
        }
    }

}
