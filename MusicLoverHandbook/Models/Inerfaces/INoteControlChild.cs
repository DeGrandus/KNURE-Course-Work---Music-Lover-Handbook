using MusicLoverHandbook.Models.Abstract;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControlChild : INoteControl
    {
        IControlParent ParentNote { get; set; }
        IControlParent GetFirstParent();
        INoteControlParent? GetFirstNoteControlParent();
    }
}