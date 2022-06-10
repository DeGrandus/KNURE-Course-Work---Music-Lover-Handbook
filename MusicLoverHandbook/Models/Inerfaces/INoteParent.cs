namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteParent<InnerNotesType> where InnerNotesType : INote
    { 
        public IReadOnlyCollection<InnerNotesType> InnerNotes { get; }

    }
}
