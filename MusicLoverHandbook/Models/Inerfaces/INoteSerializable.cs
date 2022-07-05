using MusicLoverHandbook.Models.JSON;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteSerializable
    {
        #region Public Methods

        INoteControl Clone();

        NoteImportModel Deserialize();

        string Serialize();

        #endregion Public Methods
    }
}
