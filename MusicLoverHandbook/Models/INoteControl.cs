using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models
{
    public interface INoteControl
    {
        Image? Icon { get; set; }
        string NoteDescription { get; set; }
        string NoteText { get; set; }
        int Offset { get; set; }
        NoteControlOffsetType OffsetType { get; set; }
        Color StripColor { get; set; }
        int TrueOffset { get; set; }
        Control.ControlCollection Controls { get; }
    }
}