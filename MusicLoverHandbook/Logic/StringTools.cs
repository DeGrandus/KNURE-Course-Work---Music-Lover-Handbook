using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Logic
{
    public static class StringTools
    {
        public static string[] GetTagNames(string source, char tagIdent)
        {
            return GetTagNames(source, tagIdent, false);
        }

        public static string[] GetTagNames(string source, char tagIdent, bool withIdent)
        {
            return GetTagNames(source, tagIdent, withIdent, false);
        }

        private static string[] GetTagNames(
            string source,
            char tagIdent,
            bool withIdent,
            bool withSep
        )
        {
            var sourceSplitted = source.Split(tagIdent);
            if (sourceSplitted.Length == 0)
                return new string[0];
            sourceSplitted[0] += " ";
            sourceSplitted.Where(x => x != "").ToArray();
            //Debug.WriteLine("GetTagNames");
            //Debug.WriteLine(String.Join(',', new ArraySegment<string>(sourceSplitted, 1, sourceSplitted.Length - 1).Select(s => GetTagName(s))));
            return new ArraySegment<string>(sourceSplitted, 1, sourceSplitted.Length - 1)
                .Where(x => x != "")
                .Select(s => GetTagName(s, withSep))
                .Where(x => x != "")
                .Select(x => (withIdent ? tagIdent : "") + x)
                .ToArray();
        }

        private static string GetTagName(string source, bool withSep)
        {
            var output = "";
            foreach (var ch in source)
            {
                if (output == "" && !char.IsLetterOrDigit(ch))
                    return "";
                if (ch == ':' || ch == ' ')
                    return output + (withSep ? ch : "");
                output += ch;
            }
            return output;
        }

        private static string GetTagName(string source)
        {
            return GetTagName(source, false);
        }

        public static (string Tag, string Data)[] GetTagged(string source, char tagIdent)
        {
            //source = "#test test #test ctest #test2 ctest2 #test slashn\ncont #test2 slashr2\rcontentskip #test3 multi3,data3 #test3   \n\r# test abobus #test sexy #\n#cad\r#cad2     \n";
            var tags = GetTagNames(source, tagIdent, true);
            var tagsToSearch = GetTagNames(source, tagIdent, true, true)
                .Where(x => tags.Any(y => x.Contains(y)));
            var findIn = source
                .Split(tagIdent)
                .Where(x => x != "")
                .Select(x => tagIdent + x.Split('\n')[0].Split('\r')[0]);
            var output = new List<(string, string)>();
            //Debug.WriteLine("GetTagged");
            //Debug.WriteLine(String.Join(',',tagsToSearch));
            //WriteLine(String.Join(',',findIn));
            foreach (var tag in tagsToSearch)
                output.Add(
                    (
                        tag.Substring(0, tag.Length - 1),
                        string.Join(
                            ';',
                            findIn
                                .Where(x => x.Contains(tag))
                                .Select(x => GetTagData(x))
                                .Where(x => x != "")
                        )
                    )
                );
            return output.ToArray();
        }

        public static string GetTagData(string source)
        {
            var without = source.Substring(1);
            if (without == "")
                return without;
            //Debug.WriteLine("GetTagData");
            //Debug.WriteLine(without);
            return without.Substring(GetTagName(without, true).Length).Trim();
        }
    }
}
