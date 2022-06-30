using MusicLoverHandbook.Models.Abstract;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControlParent : INoteControl
    {
        #region Public Properties

        new ObservableCollection<INoteControlChild> InnerNotes { get; set; }
        bool IsOpened { get; set; }
        ContentLinker Linker { get; }

        #endregion Public Properties

        #region Public Methods

        void AddNote(NoteControl note, ContentLinker linker);

        void AddNotes(NoteControl[] notes, ContentLinker linker);

        void MoveNote(NoteControl note, int newIndex, ContentLinker linker);

        void RemoveNote(NoteControl note, ContentLinker linker);

        void ReplaceNote(
            NoteControl oldNote,
            NoteControl newNote,
            int newIndex,
            ContentLinker linker
        );

        void ResetNotes(ContentLinker linker);

        void SwitchOpenState();

        void UpdateSize();

        #endregion Public Methods
    }
}
