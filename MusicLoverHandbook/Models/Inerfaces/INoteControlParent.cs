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
        bool IsOpened { get; set; }
        ContentLinker Linker { get; }

        void AddNote(NoteControl note);

        void MoveNote(NoteControl note, int newIndex);

        void RemoveNote(NoteControl note);

        void ReplaceNote(NoteControl oldNote, NoteControl newNote, int newIndex);

        void ResetNotes();

        void SwitchOpenState();

        void UpdateSize();
    }
}