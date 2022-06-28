using MusicLoverHandbook.Logic.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Models.JSON;
using MusicLoverHandbook.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;

namespace MusicLoverHandbook.Logic
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

        void SetMusicFilesFolderPath(string path);

        void WriteToDataFile(IParentControl parentingControl);

        void WriteToDataFile(IParentControl parentingControl, string dataFilePath);
    }

    public class FileManager : IFileManager
    {
        private Settings settings;
        public static FileManager Instance { get; }

        public string DataFilePath { get; private set; }
        public string MusicFilesFolderPath { get; private set; }

        public JsonSerializerSettings SerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver =
                        new CertainTypedContractResolver(typeof(INoteParent))
                        | new CertainTypedContractResolver(typeof(INote)),
                    Formatting = Formatting.Indented,
                };
                settings.Converters = new List<JsonConverter>()
                {
                    new StringEnumConverter(),
                    new InnerNotesConverter(settings),
                    new NoteDesrializationConverter()
                };
                return settings;
            }
        }

        static FileManager()
        {
            Instance = new FileManager();
        }

        private FileManager()
        {
            settings = Settings.Default;
            var datapath = settings.CustomDataFilePath is { Length: > 0 } dataPath
                ? dataPath
                : settings.DefaultDataFilePath;
            var musicfolder = settings.CustomMusicFilesFolderPath is { Length: > 0 } musicPath
                ? musicPath
                : settings.DefaultMusicFilesFolderPath;

            if (!Directory.Exists(musicfolder))
                Directory.CreateDirectory(musicfolder);
            MusicFilesFolderPath = musicfolder;
            if (!File.Exists(datapath))
                File.Create(datapath).Close();
            DataFilePath = datapath;
        }

        public string GetData()
        {
            return GetData(DataFilePath);
        }

        public string GetData(string dataFilePath)
        {
            using (var reader = new StreamReader(dataFilePath))
                return reader.ReadToEnd();
        }

        public string? GetMusicFilePathByName(string name)
        {
            if (!IsMusicFilesFolderValid())
                Directory.CreateDirectory(MusicFilesFolderPath);
            return Directory
                .GetFiles(MusicFilesFolderPath)
                .FirstOrDefault(x => x?.Contains(name) == true, null);
        }

        public bool IsDataFilePathDefault() => DataFilePath == settings.DefaultDataFilePath;

        public bool IsDataFileValid() => File.Exists(DataFilePath);

        public bool IsMusicFilesFolderPathDefault() =>
            MusicFilesFolderPath == settings.DefaultMusicFilesFolderPath;

        public bool IsMusicFilesFolderValid() => Directory.Exists(MusicFilesFolderPath);

        public List<NoteControl> RecreateNotesFromData()
        {
            return RecreateNotesFromData(DataFilePath);
        }

        [Obsolete(
            "Difference only in using the query LINQ expressions by newer one instead of methods in current method"
        )]
        public List<NoteControl> RecreateNotesFromData_Old(string dataFilePath)
        {
            string data = GetData(dataFilePath);
            var rawNotes =
                JsonConvert.DeserializeObject<List<NoteRawImportModel>>(data, SerializerSettings)
                ?? new();
            var noteManager = new RawNoteManager();
            var output = rawNotes.Select(x => noteManager.RecreateFromImported(x)).ToList();
            return output;
        }

        public List<NoteControl> RecreateNotesFromData(string dataFilePath) =>
            (
                from rawModel in (
                    JsonConvert.DeserializeObject<List<NoteRawImportModel>>(
                        GetData(dataFilePath),
                        SerializerSettings
                    ) ?? new()
                )
                let manager = new RawNoteManager()
                select manager.RecreateFromImported(rawModel)
            ).ToList();

        public void SetDataPath(string path)
        {
            settings.CustomDataFilePath = DataFilePath = path;
            settings.Save();
        }

        public void SetMusicFilesFolderPath(string path)
        {
            settings.CustomMusicFilesFolderPath = MusicFilesFolderPath = path;
            settings.Save();
        }

        public void WriteToDataFile(IParentControl parentingControl)
        {
            WriteToDataFile(parentingControl, DataFilePath);
        }

        public void WriteToDataFile(IParentControl parentingControl, string dataFilePath)
        {
            if (!IsDataFileValid())
                File.Create(dataFilePath).Close();
            using (var writter = new StreamWriter(dataFilePath))
                writter.Write(
                    JsonConvert.SerializeObject(parentingControl.InnerNotes, SerializerSettings)
                );
        }
    }
}
