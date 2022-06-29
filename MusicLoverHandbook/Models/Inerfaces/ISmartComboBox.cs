using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface ISmartComboBox
    {
        #region Public Properties

        NoteType InputType { get; }
        InputStatus Status { get; set; }

        #endregion Public Properties
    }
}