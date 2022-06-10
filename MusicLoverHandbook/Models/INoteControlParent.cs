using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models
{
    public interface INoteControlParent<InnerNotesType> where InnerNotesType : NoteControl
    {
        ObservableCollection<InnerNotesType> InnerNotes { get; set; }
        ContentLinker<InnerNotesType> Linker { get; }
    }
}