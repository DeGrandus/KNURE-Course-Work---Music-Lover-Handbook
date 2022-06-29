using MusicLoverHandbook.Models.Enums;
using TagFile = TagLib.File;

namespace MusicLoverHandbook.Controller
{
    public static class MusicFileScanner
    {
        public static Dictionary<NoteType, (string? Name, string? Description)> GetDataFromFile(
            string filePath
        )
        {
            if (!File.Exists(filePath) && Path.GetExtension(filePath) != "mp3")
                return null;

            var file = TagFile.Create(filePath);
            var tag = file.Tag;

            var inputNames = GetNames(filePath, file, tag);
            var inputDescriptions = GetDescriptions(filePath, file, tag);

            return Enum.GetValues<NoteType>()
                .Select(
                    t =>
                        (
                            Type: t,
                            Data: (
                                Name: inputNames.GetValueOrDefault(t),
                                Description: inputDescriptions.GetValueOrDefault(t)
                            )
                        )
                )
                .ToDictionary(k => k.Type, v => v.Data);
        }

        private static Dictionary<NoteType, string> GetDescriptions(
            string filePath,
            TagFile file,
            TagLib.Tag tag
        )
        {
            var year = tag.Year != 0 ? "Year: " + tag.Year : "";
            var comment = tag.Comment != "" ? "Comment: " + tag.Comment : "";
            var genre = tag.JoinedGenres != "" ? "Genre: " + tag.JoinedGenres : "";
            var copyright = tag.Copyright != "" ? "Copyright: " + tag.Copyright : "";
            var duration = "Duration: " + file.Properties.Duration.ToString(@"hh\:mm\:ss");

            var output = new Dictionary<NoteType, string>();
            output.Add(NoteType.Song, string.Join("\r\n", new object[] { year, genre, copyright }));
            output.Add(
                NoteType.SongFile,
                string.Join("\r\n", new object[] { filePath, duration, comment })
            );
            return output;
        }

        private static Dictionary<NoteType, string> GetNames(
            string filepath,
            TagFile file,
            TagLib.Tag tag
        )
        {
            string name = tag.Title != "" ? tag.Title : "";
            string authors = tag.JoinedPerformers != "" ? tag.JoinedPerformers : "";
            string album = tag.Album != "" ? tag.Album : "";
            string filename = Path.GetFileName(filepath);

            var output = new Dictionary<NoteType, string>();
            output.Add(NoteType.Author, authors);
            output.Add(NoteType.Disc, album);
            output.Add(NoteType.Song, name);
            output.Add(NoteType.SongFile, filename);
            return output;
        }
    }
}
