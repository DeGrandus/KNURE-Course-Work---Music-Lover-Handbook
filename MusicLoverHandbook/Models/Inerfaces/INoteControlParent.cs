using MusicLoverHandbook.Models.Abstract;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControlParent : INoteControl
    {
        ObservableCollection<INoteControlChild> InnerNotes { get; set; }
        ContentLinker Linker { get; }
        public void AddNote(NoteControl note);
        public void RemoveNote(NoteControl note);
        public void MoveNote(NoteControl note, int newIndex);
        public void ReplaceNote(NoteControl oldNote, NoteControl newNote, int newIndex);
        public void ResetNotes();
        public void UpdateSize();
    }
}
