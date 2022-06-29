using MusicLoverHandbook.Logic.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Models.JSON;
using MusicLoverHandbook.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;

namespace MusicLoverHandbook.Models
{
    public class FileManager : IFileManager
    {
        #region Private Fields

        private Settings settings;

        #endregion Private Fields

        #region Public Properties

        public static FileManager Instance { get; }

        public string DataFilePath { get; private set; }
        public HistoryManager HistoryManager => HistoryManager.Instance;
        public string MusicFilesFolderPath { get; private set; }

        public JsonSerializerSettings SerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver =
                        new TypeRestrictedContractResolver(typeof(INoteParent))
                        | new TypeRestrictedContractResolver(typeof(INote)),
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

        public bool SaveOnClose
        {
            get => settings.SaveOnClose; set
            {
                settings.SaveOnClose = value;
                settings.Save();
            }
        }

        #endregion Public Properties



        #region Public Constructors

        static FileManager()
        {
            Instance = new FileManager();
        }

        #endregion Public Constructors



        #region Private Constructors

        private FileManager()
        {
            settings = Settings.Default;
            var datapath = settings.CustomDataFilePath is { Length: > 0 } dataPath
                ? dataPath
                : settings.DefaultDataFilePath;
            var musicFolder = settings.CustomMusicFilesFolderPath is { Length: > 0 } musicPath
                ? musicPath
                : settings.DefaultMusicFilesFolderPath;

            if (!Directory.Exists(musicFolder))
                try
                {
                    Directory.CreateDirectory(musicFolder);
                }
                catch
                {
                    ResetMusicFilesFolderPathToDefalut();
                    Directory.CreateDirectory(MusicFilesFolderPath!);
                }
            MusicFilesFolderPath = musicFolder;
            if (!File.Exists(datapath))
                try
                {
                    File.Create(datapath).Close();
                }
                catch
                {
                    ResetMusicFilesFolderPathToDefalut();
                    File.Create(DataFilePath!).Close();
                }

            DataFilePath = datapath;
        }

        #endregion Private Constructors



        #region Public Methods

        public bool CheckMusicFilePathOrName(string filePath)
        {
            Debug.WriteLine(filePath);
            Debug.WriteLine(Path.GetDirectoryName(filePath));
            if (GetMusicFilePathByName(filePath) != null)
                return true;
            if (!File.Exists(filePath))
                return false;
            if (
                Path.GetFullPath(Path.GetDirectoryName(Path.GetFullPath(filePath))!)
                    .TrimEnd('\\', '/')
                    .ToLower()
                == Path.GetFullPath(MusicFilesFolderPath).TrimEnd('\\', '/').ToLower()
            )
                return true;
            return false;
        }

        public string CopyToMusicFolder(string filePath)
        {
            var newPath = Path.Combine(MusicFilesFolderPath, Path.GetFileName(filePath));
            File.Copy(filePath, Path.Combine(MusicFilesFolderPath, Path.GetFileName(filePath)));
            return newPath;
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
                .FirstOrDefault(
                    x =>
                    {
                        return Path.GetFileNameWithoutExtension(x) is var ex
                            && ex == Path.GetFileNameWithoutExtension(name) == true;
                    },
                    null
                );
        }

        public bool IsDataFilePathDefault() => DataFilePath == settings.DefaultDataFilePath;

        public bool IsDataFileValid() => File.Exists(DataFilePath);

        public bool IsMusicFilesFolderPathDefault() =>
            MusicFilesFolderPath == settings.DefaultMusicFilesFolderPath;

        public bool IsMusicFilesFolderValid() => Directory.Exists(MusicFilesFolderPath);

        public string MoveToMusicFolder(string filePath)
        {
            var newPath = Path.Combine(MusicFilesFolderPath, Path.GetFileName(filePath));
            File.Move(filePath, Path.Combine(MusicFilesFolderPath, Path.GetFileName(filePath)));
            return newPath;
        }

        public List<NoteControl> RecreateNotesFromData()
        {
            return RecreateNotesFromData(DataFilePath);
        }

        public List<NoteControl> RecreateNotesFromData(string dataFilePath) =>
            (
                from rawModel in JsonConvert.DeserializeObject<List<NoteRawImportModel>>(
                    GetData(dataFilePath),
                    SerializerSettings
                ) ?? new()
                let manager = new RawNoteManager()
                select manager.RecreateFromImported(rawModel)
            ).ToList();

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

        public void ResetDataFilePathToDefault() => SetDataPath("");

        public void ResetMusicFilesFolderPathToDefalut() => SetMusicFilesFolderPath("");

        public void SetDataPath(string path)
        {
            settings.CustomDataFilePath = "";
            if (path != "")
                settings.CustomDataFilePath = DataFilePath = path;
            else
                DataFilePath = settings.DefaultDataFilePath;
            settings.Save();
        }

        public void SetMusicFilesFolderPath(string path)
        {
            settings.CustomMusicFilesFolderPath = "";
            if (path != "")
                settings.CustomMusicFilesFolderPath = MusicFilesFolderPath = path;
            else
                MusicFilesFolderPath = settings.DefaultMusicFilesFolderPath;
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

        #endregion Public Methods
    }
}
