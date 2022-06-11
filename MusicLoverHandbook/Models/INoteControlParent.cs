using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models
{
    public interface INoteControlParent<InnerNotesType> : INoteControl where InnerNotesType : INoteControl
    {
        ObservableCollection<InnerNotesType> InnerNotes { get; set; }
        ContentLinker<InnerNotesType> Linker { get; }
        public void AddNote(NoteControl note);
        public void RemoveNote(NoteControl note);
        public void MoveNote(NoteControl note, int newIndex);
        public void ReplaceNote(NoteControl oldNote, NoteControl newNote, int newIndex);
        public void ResetNotes();
        public void UpdateSize();
    }
}