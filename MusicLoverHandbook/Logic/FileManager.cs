using MusicLoverHandbook.Logic.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Models.JSON;
using MusicLoverHandbook.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

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

        string MoveToMusicFolder(string filePath);
        void SetMusicFilesFolderPath(string path);

        void WriteToDataFile(IParentControl parentingControl);

        void WriteToDataFile(IParentControl parentingControl, string dataFilePath);
        string CopyToMusicFolder(string filePath);
        bool CheckMusicFilePathOrName(string filePath);
        HistoryManager HistoryManager { get; }
    }
    public class HistoryManager
    {
        public static HistoryManager Instance;
        static HistoryManager() => Instance = new HistoryManager();
        private HistoryManager()
        {
            var path = "HandbookHistory.json";

            HistoryFileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 4096, FileOptions.DeleteOnClose);
        }
        public FileStream HistoryFileStream { get; }
        public int MaxHistoryLength { get; } = 10;
        public int CurrentHistoryBranchIndex { get; private set; } = 0;
        public int CurrentHistoryLength { get; private set; } = 0;
        public List<NoteControl>? UndoNotes()
        {
            if (CurrentHistoryBranchIndex == 0)
                return null;
            CurrentHistoryBranchIndex--;

            return GetHistoryBranch();
        }
        public List<NoteControl>? RedoNotes()
        {
            if (CurrentHistoryBranchIndex == MaxHistoryLength-1 || CurrentHistoryLength - 1 == CurrentHistoryBranchIndex)
                return null;
           
            CurrentHistoryBranchIndex++;

            return GetHistoryBranch();
        }
        public void UpdateHistory(IParentControl container)
        {
            WriteHistory(container.InnerNotes.Cast<NoteControl>().ToList());
        }
        private List<NoteControl>? GetHistoryBranch()
        {
            var history = ReadHistory();
            var rawHistoryBranch = history[CurrentHistoryBranchIndex].ToObject<List<NoteRawImportModel>>(JsonSerializer.Create(FileManager.Instance.SerializerSettings));
            return rawHistoryBranch!.Select(b=> new RawNoteManager().RecreateFromImported(b)).ToList();
        }
        public List<JArray> ReadHistory()
        {
            using (var reader = new StreamReader(HistoryFileStream, Encoding.UTF8, true, 4096, true))
            {
                HistoryFileStream.Seek(0, SeekOrigin.Begin);
                var readed = reader.ReadToEnd();
                Debug.WriteLine(readed);
                return JsonConvert.DeserializeObject<List<JArray>>(readed) ?? new();
            }
        }
        public void WriteHistory(List<NoteControl> notes)
        {
            var history = ReadHistory();
            var historyBranch = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(notes,FileManager.Instance.SerializerSettings));

            history = history.SkipLast(CurrentHistoryLength>0?CurrentHistoryLength- 1-CurrentHistoryBranchIndex:0).Skip(history.Count < MaxHistoryLength ? 0 : 1).Concat(new JArray[] { historyBranch! }).ToList();
            CurrentHistoryLength = history.Count;
            CurrentHistoryBranchIndex = history.Count - 1;

            using (var writer = new StreamWriter(HistoryFileStream,null,-1,true))
            {
                HistoryFileStream.Seek(0, SeekOrigin.Begin);

                var serializedHistory = JsonConvert.SerializeObject(history, FileManager.Instance.SerializerSettings);
                HistoryFileStream.SetLength(0);
                writer.Write(serializedHistory);
            }
        }
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
        public HistoryManager HistoryManager => HistoryManager.Instance;

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

        public string MoveToMusicFolder(string filePath)
        {
            var newPath = Path.Combine(MusicFilesFolderPath, Path.GetFileName(filePath));
            File.Move(filePath, Path.Combine(MusicFilesFolderPath, Path.GetFileName(filePath)));
            return newPath;
        }
        public string CopyToMusicFolder(string filePath)
        {
            var newPath = Path.Combine(MusicFilesFolderPath, Path.GetFileName(filePath));
            File.Copy(filePath, Path.Combine(MusicFilesFolderPath, Path.GetFileName(filePath)));
            return newPath;
        }

        public bool CheckMusicFilePathOrName(string filePath)
        {
            if (GetMusicFilePathByName(filePath) != null)
                return true;
            if (!File.Exists(filePath))
                return false;
            if (Path.GetFullPath(Path.GetDirectoryName(filePath)!).TrimEnd('\\', '/').ToLower() == Path.GetFullPath(MusicFilesFolderPath).TrimEnd('\\', '/').ToLower())
                return true;
            return false;
        }
    }
}
