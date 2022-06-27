using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface ISmartComboBox
    {
        NoteType InputType { get; }
        InputStatus Status { get; set; }
    }
}