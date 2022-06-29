using MusicLoverHandbook.Logic.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Models.JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

namespace MusicLoverHandbook.Models
{
    public class HistoryManager
    {
        public static HistoryManager Instance;

        static HistoryManager() => Instance = new HistoryManager();

        private HistoryManager()
        {
            var path = "HandbookHistory.json";

            HistoryFileStream = new FileStream(
                path,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite,
                4096,
                FileOptions.DeleteOnClose
            );
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
            if (
                CurrentHistoryBranchIndex == MaxHistoryLength - 1
                || CurrentHistoryLength - 1 == CurrentHistoryBranchIndex
            )
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
            var rawHistoryBranch = history[CurrentHistoryBranchIndex].ToObject<
                List<NoteRawImportModel>
            >(JsonSerializer.Create(FileManager.Instance.SerializerSettings));
            return rawHistoryBranch!
                .Select(b => new RawNoteManager().RecreateFromImported(b))
                .ToList();
        }

        public List<JArray> ReadHistory()
        {
            using (
                var reader = new StreamReader(HistoryFileStream, Encoding.UTF8, true, 4096, true)
            )
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
            var historyBranch = JsonConvert.DeserializeObject<JArray>(
                JsonConvert.SerializeObject(notes, FileManager.Instance.SerializerSettings)
            );

            history = history
                .SkipLast(
                    CurrentHistoryLength > 0
                      ? CurrentHistoryLength - 1 - CurrentHistoryBranchIndex
                      : 0
                )
                .Skip(history.Count < MaxHistoryLength ? 0 : 1)
                .Concat(new JArray[] { historyBranch! })
                .ToList();
            CurrentHistoryLength = history.Count;
            CurrentHistoryBranchIndex = history.Count - 1;

            using (var writer = new StreamWriter(HistoryFileStream, null, -1, true))
            {
                HistoryFileStream.Seek(0, SeekOrigin.Begin);

                var serializedHistory = JsonConvert.SerializeObject(
                    history,
                    FileManager.Instance.SerializerSettings
                );
                HistoryFileStream.SetLength(0);
                writer.Write(serializedHistory);
            }
        }
    }
}
