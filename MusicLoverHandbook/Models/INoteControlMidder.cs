using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models
{
    public interface INoteControlMidder<ParentNoteType, InnerNotesType>
        : INoteControlChild<ParentNoteType>,
          INoteControlParent<InnerNotesType>,
          INoteMidder<ParentNoteType, InnerNotesType>
        where ParentNoteType : INoteControlParent<INoteControl>
        where InnerNotesType : INoteControl { }
}
