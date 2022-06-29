using MusicLoverHandbook.Models.Abstract;
using Newtonsoft.Json;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface IFileManager
    {
        string DataFilePath { get; }
        string MusicFilesFolderPath { get; }

        JsonSerializerSettings SerializerSettings { get; }

        string GetData();
        string GetData(string dataFilePath);

        string? GetMusicFilePathByName(string name);

        bool IsDataFilePathDefault();

        bool IsDataFileValid();

        bool IsMusicFilesFolderPathDefault();

        bool IsMusicFilesFolderValid();

        List<NoteControl> RecreateNotesFromData();

        List<NoteControl> RecreateNotesFromData(string dataFilePath);

        void SetDataPath(string path);

        string MoveToMusicFolder(string filePath);
        void SetMusicFilesFolderPath(string path);

        void WriteToDataFile(IParentControl parentingControl);

        void WriteToDataFile(IParentControl parentingControl, string dataFilePath);
        string CopyToMusicFolder(string filePath);
        bool CheckMusicFilePathOrName(string filePath);

        void ResetDataFilePathToDefault();
        void ResetMusicFilesFolderPathToDefalut();
        HistoryManager HistoryManager { get; }
    }
}
