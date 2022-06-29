using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INote
    {
        #region Public Properties

        string NoteDescription { get; set; }
        string NoteName { get; set; }
        NoteType NoteType { get; }
        NoteCreationOrder? UsedCreationOrder { get; }

        #endregion Public Properties
    }
}
