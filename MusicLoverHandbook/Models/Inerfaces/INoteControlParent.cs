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

        void AddNote(NoteControl note, ContentLinker linker);
        void AddNotes(NoteControl[] notes, ContentLinker linker);

        void MoveNote(NoteControl note, int newIndex, ContentLinker linker);

        void RemoveNote(NoteControl note, ContentLinker linker);

        void ReplaceNote(NoteControl oldNote, NoteControl newNote, int newIndex, ContentLinker linker);

        void ResetNotes(ContentLinker linker);

        void SwitchOpenState();

        void UpdateSize();
    }
}
