using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models
{
    public abstract class NoteControlParent<InnerNotesType> : NoteControl, INoteParent<InnerNotesType>, INoteControlParent<InnerNotesType> where InnerNotesType : INoteControl
    {
        public ObservableCollection<InnerNotesType> InnerNotes { get; set; } = new();
        public ContentLinker<InnerNotesType> Linker { get; }
        IReadOnlyCollection<InnerNotesType> INoteParent<InnerNotesType>.InnerNotes => InnerNotes.ToList();

        protected NoteControlParent(string text, string description) : base(text, description)
        {
            Linker = new ContentLinker<InnerNotesType>(this);
        }
    }

}
