using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface ISmartComboBox
    {
        InputState Status { get; set; }
        InputType InputType { get; }
    }
}
