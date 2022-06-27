using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Models.JSON;
using MusicLoverHandbook.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MusicLoverHandbook.Logic
{
    public interface IFileManager
    {
        string DataFilePath { get; }
        string MusicFilesFolderPath { get; }

        JsonSerializerSettings SerializerSettings { get; }

        string GetData();

        string GetMusicFilePathByName(string name);

        bool IsDataFilePathDefault();

        bool IsDataFileValid();

        bool IsMusicFilesFolderPathDefault();

        bool IsMusicFilesFolderValid();

        NoteControl[] RecreateNotesFromData();

        NoteControl[] RecreateNotesFromData(string dataFilePath);

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
                    ContractResolver = new CertainTypedContractResolver(typeof(INoteParent)) | new CertainTypedContractResolver(typeof(INote)),
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

            Directory.CreateDirectory(musicfolder);
            MusicFilesFolderPath = musicfolder;
            File.Create(datapath);
            DataFilePath = datapath;
        }

        public string GetData()
        {
            throw new NotImplementedException();
        }

        public string GetMusicFilePathByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool IsDataFilePathDefault() => DataFilePath == settings.DefaultDataFilePath;

        public bool IsDataFileValid() => File.Exists(DataFilePath);

        public bool IsMusicFilesFolderPathDefault() =>
            MusicFilesFolderPath == settings.DefaultMusicFilesFolderPath;

        public bool IsMusicFilesFolderValid() => Directory.Exists(MusicFilesFolderPath);

        public NoteControl[] RecreateNotesFromData()
        {
            throw new NotImplementedException();
        }

        public NoteControl[] RecreateNotesFromData(string dataFilePath)
        {
            throw new NotImplementedException();
        }

        public void SetDataPath(string path) => settings.CustomDataFilePath = DataFilePath = path;

        public void SetMusicFilesFolderPath(string path) =>
            settings.CustomMusicFilesFolderPath = MusicFilesFolderPath = path;

        public void WriteToDataFile(IParentControl parentingControl)
        {
            throw new NotImplementedException();
        }

        public void WriteToDataFile(IParentControl parentingControl, string dataFilePath)
        {
            if (!IsDataFileValid())
                File.Create(dataFilePath);
            using (var writter = new StreamWriter(dataFilePath))
                writter.Write(JsonConvert.SerializeObject(parentingControl.InnerNotes, SerializerSettings));
        }
    }
}