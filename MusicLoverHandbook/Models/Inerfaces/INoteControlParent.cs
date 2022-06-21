using MusicLoverHandbook.Models.Abstract;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface IControlParent
    {
        ObservableCollection<INoteControlChild> InnerNotes { get; }
    }

    public interface INoteControlParent : INoteControl, IControlParent
    {
        ContentLinker Linker { get; }
        bool IsOpened { get; set; }
        void AddNote(NoteControl note);
        void RemoveNote(NoteControl note);
        void MoveNote(NoteControl note, int newIndex);
        void ReplaceNote(NoteControl oldNote, NoteControl newNote, int newIndex);
        void ResetNotes();
        void UpdateSize();
        void SwitchOpenState();
    }
}
