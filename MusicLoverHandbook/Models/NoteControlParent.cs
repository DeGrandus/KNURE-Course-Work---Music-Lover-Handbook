using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models
{
    public class NoteControlParent<InnerNotesType> : NoteControl, INoteParent<InnerNotesType>, INoteControlParent<InnerNotesType> where InnerNotesType : NoteControl
    {
        public ObservableCollection<InnerNotesType> InnerNotes { get; set; } = new();
        public ContentLinker<InnerNotesType> Linker { get; }
        IReadOnlyCollection<InnerNotesType> INoteParent<InnerNotesType>.InnerNotes => InnerNotes.ToList();

        protected NoteControlParent(Color stripColor, string text, string description) : base(stripColor, text, description)
        {
            Linker = new ContentLinker<InnerNotesType>(this);
        }
    }

}
