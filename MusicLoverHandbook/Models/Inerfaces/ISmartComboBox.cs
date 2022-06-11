using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface ISmartComboBox
    {
        InputState State { get; set; }
        InputType InputType { get; }
    }
}
