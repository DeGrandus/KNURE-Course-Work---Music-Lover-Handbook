using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Managers;
using Newtonsoft.Json;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface IFileManager
    {
        #region Public Properties

        string DataFilePath { get; }
        HistoryManager HistoryManager { get; }
        string MusicFilesFolderPath { get; }
        bool SaveOnClose { get; }
        JsonSerializerSettings SerializerSettings { get; }

        #endregion Public Properties

        #region Public Methods

        bool CheckMusicFilePathOrName(string filePath);

        string CopyToMusicFolder(string filePath);

        string GetData();

        string GetData(string dataFilePath);

        string? GetMusicFilePathByName(string name);

        bool IsDataFilePathDefault();

        bool IsDataFileValid();

        bool IsMusicFilesFolderPathDefault();

        bool IsMusicFilesFolderValid();

        string MoveToMusicFolder(string filePath);

        List<NoteControl> RecreateNotesFromData();

        List<NoteControl> RecreateNotesFromData(string dataFilePath);

        void ResetDataFilePathToDefault();

        void ResetMusicFilesFolderPathToDefalut();

        void SetDataPath(string path);

        void SetMusicFilesFolderPath(string path);

        void WriteToDataFile(IParentControl parentingControl);

        void WriteToDataFile(IParentControl parentingControl, string dataFilePath);

        #endregion Public Methods
    }
}