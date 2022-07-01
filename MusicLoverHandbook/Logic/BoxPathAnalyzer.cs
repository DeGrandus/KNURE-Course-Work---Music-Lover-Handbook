using MusicLoverHandbook.Models.Delegates;
using MusicLoverHandbook.Models.Managers;
using System.Text.RegularExpressions;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls
{
    public partial class BoxPathAnalyzer : IDisposable
    {
        #region Private Fields

        private TextBox box;

        private FileSystemWatcher? defaultWatcher;

        private FileSystemWatcher? observedFileWatcher;

        private PathAnalyzerResultHandler? resultsChanged;

        #endregion Private Fields

        #region Public Constructors

        public BoxPathAnalyzer(TextBox analyzeBox)
        {
            box = analyzeBox;
            box.TextChanged += OnBoxTextChanged;
            defaultWatcher = new(
                Path.GetFullPath(FileManager.Instance.MusicFilesFolderPath),
                "*.mp3"
            )
            {
                EnableRaisingEvents = true
            };
            observedFileWatcher = new FileSystemWatcher()
            {
                EnableRaisingEvents = false,
                Path = ""
            };
            Setup_WatcherEvents(defaultWatcher);
            Setup_WatcherEvents(observedFileWatcher);
        }

        #endregion Public Constructors

        #region Public Methods

        public void AnalyzeBoxText()
        {
            var obeservedString = Regex
                .Replace(box.Text, @"(\n|\r)+", "\r\n")
                .Split("\r\n")
                .FirstOrDefault("");
            OnResultsChange(StringAnalyzer(obeservedString), obeservedString);
        }

        public void Dispose()
        {
            box.TextChanged -= OnBoxTextChanged;
            resultsChanged = null;
            if (defaultWatcher == null && observedFileWatcher == null)
                return;

            Dispose_Watcher(defaultWatcher);
            Dispose_Watcher(observedFileWatcher);

            defaultWatcher = null;
            observedFileWatcher = null;
        }

        public void ReplacePath(string newPath)
        {
            box.Text = string.Join(
                "\r\n",
                Regex
                    .Replace(box.Text, @"(\n|\r)+", "\r\n")
                    .Split("\r\n")
                    .Skip(1)
                    .Reverse()
                    .Concat(new[] { newPath })
                    .Reverse()
            );
        }

        #endregion Public Methods

        #region Private Methods

        private void Dispose_Watcher(FileSystemWatcher? watcher)
        {
            if (watcher == null)
                return;
            watcher.EnableRaisingEvents = false;
            watcher!.Changed -= OnDefaultMusicFolderContentChanged;
            watcher!.Renamed -= OnDefaultMusicFolderContentChanged;
            watcher!.Deleted -= OnDefaultMusicFolderContentChanged;
            watcher!.Created -= OnDefaultMusicFolderContentChanged;
            watcher.Dispose();
        }

        private void OnBoxTextChanged(object? sender, EventArgs e) => AnalyzeBoxText();

        private void OnDefaultMusicFolderContentChanged(object sender, FileSystemEventArgs e) =>
            box.Invoke(() => AnalyzeBoxText());

        private void OnResultsChange(PathAnalyzerResult pathAnalyzerResult, string obeservedString)
        {
            if (resultsChanged != null)
                resultsChanged(pathAnalyzerResult, obeservedString);
        }

        private void Setup_WatcherEvents(FileSystemWatcher watcher)
        {
            watcher.Created += OnDefaultMusicFolderContentChanged;
            watcher.Changed += OnDefaultMusicFolderContentChanged;
            watcher.Renamed += OnDefaultMusicFolderContentChanged;
            watcher.Deleted += OnDefaultMusicFolderContentChanged;
        }
        private PathAnalyzerResult analyzingResult;
        private PathAnalyzerResult StringAnalyzer(string observedString)
        {
            if (FileManager.Instance.CheckMusicFilePathOrName(observedString))
            {
                ReplacePath(Path.GetFileName(observedString));
                return analyzingResult = PathAnalyzerResult.File_InMusicFolder;
            }
            if (!Path.IsPathRooted(observedString))
                return analyzingResult = PathAnalyzerResult.IsNotAFile;
            if (!File.Exists(observedString))
                return analyzingResult = PathAnalyzerResult.File_DoesNotExist;
            if (Path.GetExtension(observedString).ToLower() != ".mp3")
                return analyzingResult = PathAnalyzerResult.File_NotMp3;

            observedFileWatcher!.Path = Path.GetFullPath(Path.GetDirectoryName(observedString)!);
            observedFileWatcher.Filter = Path.GetFileName(observedString);
            observedFileWatcher.EnableRaisingEvents = true;
            if (
                Path.GetFullPath(Path.GetDirectoryName(observedString) ?? "???")
                    .TrimEnd('\\', '/')
                    .ToLower()
                != Path.GetFullPath(FileManager.Instance.MusicFilesFolderPath)
                    .TrimEnd('\\', '/')
                    .ToLower()
            )
                if (
                    Directory
                        .GetFiles(FileManager.Instance.MusicFilesFolderPath + '\\', "*.mp3")
                        .Any(f => Path.GetFileName(f) == Path.GetFileName(observedString))
                )
                    return analyzingResult = PathAnalyzerResult.File_HasEquivalence;
                else
                    return analyzingResult = PathAnalyzerResult.File_NotInMusicFolder;

            return analyzingResult = PathAnalyzerResult.File_InMusicFolder;
        }

        #endregion Private Methods

        #region Public Delegates

        

        #endregion Public Delegates

        #region Public Events

        public event PathAnalyzerResultHandler ResultsChanged
        {
            add => resultsChanged += value;
            remove => resultsChanged -= value;
        }

        #endregion Public Events
    }
}
