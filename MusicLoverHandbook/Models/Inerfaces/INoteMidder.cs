namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteMidder<ParentNoteType, InnerNotesType> : INoteChild<ParentNoteType>, INoteParent<InnerNotesType> where ParentNoteType : INote where InnerNotesType : INote
    {

    }
}
