namespace MusicLoverHandbook.Models
{
    public interface INoteControlChild<ParentNoteType> : INoteControl
        where ParentNoteType : INoteControl
    {
        ParentNoteType ParentNote { get; set; }
    }
}
