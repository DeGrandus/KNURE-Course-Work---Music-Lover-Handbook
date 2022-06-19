using MusicLoverHandbook.Models.Enums;
using TagFile = TagLib.File;

namespace MusicLoverHandbook.Controller
{
    public static class MusicFileScanner
    {
        public static Dictionary<InputType, (string? Name, string? Description)> GetDataFromFile(
            string filePath
        )
        {
            if (!File.Exists(filePath) && Path.GetExtension(filePath) != "mp3")
                return null;

            var file = TagFile.Create(filePath);
            var tag = file.Tag;

            var inputNames = GetNames(filePath, file, tag);
            var inputDescriptions = GetDescriptions(filePath, file, tag);

            return Enum.GetValues<InputType>()
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

            //allInputs.ForEach(x => x.AutoFill = false);
            //if (
            //    allInputs
            //        .Select(x => x.InputNameBox.Status)
            //        .Where(x => x == InputState.OK || x == InputState.CREATION)
            //        .Count() > 1
            //)
            //{
            //    var result = MessageBox.Show(
            //        "There are some fields that are already filled with some data. Replace with file data (YES) or fill up only remain data (NO)",
            //        "Information load question",
            //        MessageBoxButtons.YesNo,
            //        MessageBoxIcon.Question
            //    );
            //    if (result != DialogResult.Cancel)
            //    {
            //        if (result == DialogResult.Yes)
            //        {
            //            inputAuthor.InputNameBox.Text = authors;
            //            inputDisc.InputNameBox.Text = album;
            //            inputSong.InputNameBox.Text = name;
            //            inputSong.InputDescriptionBox.Text = descSong;
            //        }
            //        if (
            //            inputSong.InputNameBox.Status is InputState x
            //            && !(x == InputState.OK || x == InputState.CREATION)
            //        )
            //            inputSong.InputNameBox.Text = name;
            //        inputSongFile.InputNameBox.Text = filename;
            //        inputSongFile.InputDescriptionBox.Text = descFile;
            //    }
            //}
            //else
            //{
            //    inputAuthor.InputNameBox.Text = authors;
            //    inputDisc.InputNameBox.Text = album;
            //    inputSong.InputNameBox.Text = name;
            //    inputSong.InputDescriptionBox.Text = descSong;
            //    inputSongFile.InputNameBox.Text = filename;
            //    inputSongFile.InputDescriptionBox.Text = descFile;
            //}

            //allInputs.ForEach(
            //    x =>
            //    {
            //        x.InputNameBox.CheckValid();
            //    }
            //);
        }

        private static Dictionary<InputType, string> GetDescriptions(
            string filepath,
            TagFile file,
            TagLib.Tag tag
        )
        {
            var year = tag.Year != 0 ? "Year: " + tag.Year : "";
            var comment = tag.Comment != "" ? "Comment: " + tag.Comment : "";
            var genre = tag.JoinedGenres != "" ? "Genre: " + tag.JoinedGenres : "";
            var copyright = tag.Copyright != "" ? "Copyright: " + tag.Copyright : "";
            var duration = "Duration: " + file.Properties.Duration.ToString(@"hh\:mm\:ss");

            var ret = new Dictionary<InputType, string>();
            ret.Add(
                InputType.SongName,
                string.Join("\r\n", new object[] { year, genre, copyright })
            );
            ret.Add(
                InputType.SongFile,
                string.Join("\r\n", new object[] { filepath, duration, comment })
            );
            return ret;
        }

        private static Dictionary<InputType, string> GetNames(
            string filepath,
            TagFile file,
            TagLib.Tag tag
        )
        {
            string name = tag.Title != "" ? tag.Title : "";
            string authors = tag.JoinedPerformers != "" ? tag.JoinedPerformers : "";
            string album = tag.Album != "" ? tag.Album : "";
            string filename = Path.GetFileName(filepath);

            var ret = new Dictionary<InputType, string>();
            ret.Add(InputType.Author, authors);
            ret.Add(InputType.Disc, album);
            ret.Add(InputType.SongName, name);
            ret.Add(InputType.SongFile, filename);
            return ret;
        }
    }
}
