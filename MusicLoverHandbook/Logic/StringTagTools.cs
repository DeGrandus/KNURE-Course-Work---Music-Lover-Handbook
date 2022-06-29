using System.Text.RegularExpressions;

namespace MusicLoverHandbook.Logic
{
    public static partial class StringTagTools
    {
        //private static string teststring =
        //    ""
        //    + "#test test\n\r"
        //    + "#test2: test2\n"
        //    + "#test3 :test3\r"
        //    + "#test4     testSpaces sentance continues "
        //    + "#test5 newSentance no new line \r "
        //    + "# general and named\r"
        //    + "skip this "
        //    + "#emptywithseparator\r"
        //    + "#empty"
        //    + "#emptyspace "
        //    + "#\r"
        //    + "#\n"
        //    + "# \n"
        //    + "# #####justempty";

        #region Public Methods

        public static Dictionary<TagName, TagValue> GetTagged(string source, char tagMarker)
        {
            if (!source.Contains(tagMarker))
                return new();
            var output = new Dictionary<TagName, TagValue>();
            source = Regex.Replace(source, "(#)+", "$1");
            source = Regex.Replace(source, "( )+", "$1");
            source = Regex.Replace(source, "^#", " #");
            var sep = source
                .Split(tagMarker)
                .Skip(1)
                .Select(x => x.Split('\n')[0].Split('\r')[0])
                .Select(x => x == "" ? " " : x);
            var tagReg = new Regex(@"(^ |(?:[a-zа-я'їіє\w])+) ?:", RegexOptions.IgnoreCase);
            foreach (var tagdata in sep)
            {
                var tagdataf = tagReg.Replace(tagdata, "$1 ", 1);
                var wds = Regex
                    .Matches(
                        tagdataf,
                        @"(^ |(?:[a-zа-я'їіє\w])+)(?= )?(?=:)?",
                        RegexOptions.IgnoreCase
                    )
                    .Select(x => x.Value);
                var tagname = wds.FirstOrDefault("");
                var tagvalue = string.Join(tagname, tagdataf.Split(tagname).Skip(1)).Trim();
                tagname = tagname.Trim();
                var name = new TagName(
                    tagname.Trim() == "" ? TagDataType.General : TagDataType.Valued,
                    tagname
                );
                var value = new TagValue(
                    tagvalue.Trim() == "" ? TagDataType.General : TagDataType.Valued,
                    tagvalue
                );
                output.Add(name, value);
            }
            return output;
        }

        #endregion Public Methods
    }
}