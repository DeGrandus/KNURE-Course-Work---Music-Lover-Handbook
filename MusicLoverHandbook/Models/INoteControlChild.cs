namespace MusicLoverHandbook.Models
{
    public interface INoteControlChild<ParentNoteType> : INoteControl
        where ParentNoteType : INoteControlParent<INoteControl>
    {
        ParentNoteType ParentNote { get; set; }
    }
}
