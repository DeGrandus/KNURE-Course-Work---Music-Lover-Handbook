using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models
{
    public interface INoteControlParent<InnerNotesType> : INoteControl where InnerNotesType : INoteControl
    {
        ObservableCollection<InnerNotesType> InnerNotes { get; set; }
        ContentLinker<InnerNotesType> Linker { get; }
    }
}