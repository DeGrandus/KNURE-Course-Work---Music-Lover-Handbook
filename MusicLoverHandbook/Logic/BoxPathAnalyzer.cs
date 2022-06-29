﻿using MusicLoverHandbook.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls
{
    public class BoxPathAnalyzer : IDisposable
    {
        private TextBox box;

        private void OnBoxTextChanged(object? sender, EventArgs e) => AnalyzeBoxText();

        public void AnalyzeBoxText()
        {
            var obeservedString = Regex
                .Replace(box.Text, @"(\n|\r)+", "\r\n")
                .Split("\r\n")
                .FirstOrDefault("");
            OnResultsChange(StringAnalyzer(obeservedString), obeservedString);
        }

        private FileSystemWatcher? defaultWatcher;

        private PathAnalyzerResult StringAnalyzer(string observedString)
        {
            if (FileManager.Instance.CheckMusicFilePathOrName(observedString))
            {
                ReplacePath(Path.GetFileName(observedString));
                return PathAnalyzerResult.FileInDefault;
            }
            if (!Path.IsPathRooted(observedString))
                return PathAnalyzerResult.NotAFile;
            if (!File.Exists(observedString))
                return PathAnalyzerResult.FileNotExist;
            if (Path.GetExtension(observedString).ToLower() != ".mp3")
                return PathAnalyzerResult.FileNotMp3;

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
                    return PathAnalyzerResult.FileHasEquivalence;
                else
                    return PathAnalyzerResult.FileNotInDefault;

            return PathAnalyzerResult.FileInDefault;
        }

        private FileSystemWatcher? observedFileWatcher;

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

        private void Setup_WatcherEvents(FileSystemWatcher watcher)
        {
            watcher.Created += OnDefaultMusicFolderContentChanged;
            watcher.Changed += OnDefaultMusicFolderContentChanged;
            watcher.Renamed += OnDefaultMusicFolderContentChanged;
            watcher.Deleted += OnDefaultMusicFolderContentChanged;
        }

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

        private void OnDefaultMusicFolderContentChanged(object sender, FileSystemEventArgs e) =>
            box.Invoke(() => AnalyzeBoxText());

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

        [Flags]
        public enum PathAnalyzerResult
        {
            FileNotMp3 = 1,
            FileHasEquivalence = 2,
            FileNotExist = 4,
            FileInDefault = 8,
            FileNotInDefault = 16,
            NotAFile = 32,
        }

        private void OnResultsChange(PathAnalyzerResult pathAnalyzerResult, string obeservedString)
        {
            if (resultsChanged != null)
                resultsChanged(pathAnalyzerResult, obeservedString);
            Debug.WriteLine(pathAnalyzerResult);
        }

        public delegate void PathAnalyzerResultHandler(
            PathAnalyzerResult result,
            string obeservedString
        );
        private PathAnalyzerResultHandler? resultsChanged;
        public event PathAnalyzerResultHandler ResultsChanged
        {
            add => resultsChanged += value;
            remove => resultsChanged -= value;
        }
    }
}
