using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface ISmartComboBox
    {
        InputStatus Status { get; set; }
        InputType InputType { get; }
    }
}
