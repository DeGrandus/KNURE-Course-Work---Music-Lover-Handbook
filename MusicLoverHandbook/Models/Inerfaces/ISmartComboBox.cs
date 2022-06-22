using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface ISmartComboBox
    {
        InputType InputType { get; }
        InputStatus Status { get; set; }
    }
}