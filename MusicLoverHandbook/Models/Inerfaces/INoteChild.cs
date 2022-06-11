namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteChild<ParentNoteType> where ParentNoteType : INote
    {
        public ParentNoteType ParentNote { get; }
    }
}
