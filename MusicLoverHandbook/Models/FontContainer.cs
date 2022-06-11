using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models
{
    public class FontContainer
    {
        public PrivateFontCollection Fonts { get; set; }
        public static FontContainer Instance { get; }
        static FontContainer()
        {
            Instance = new FontContainer();
        }
        public FontContainer()
        {
            Fonts = new PrivateFontCollection();
            var temppath = Path.GetTempPath();
            var executing = Assembly.GetExecutingAssembly();
            var fonts = executing.GetManifestResourceNames().Where(x => Regex.IsMatch(x, @"\.tff|\.otf"));
            foreach (var font in fonts)
            {
                var fullpath = Path.Combine(temppath, font);
                if (!File.Exists(fullpath))
                {
                    var stream = executing.GetManifestResourceStream(font);
                    if (stream == null) continue;

                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)bytes.Length);
                    stream.Close();

                    using (var fileStream = File.OpenWrite(fullpath))
                    using (var writer = new BinaryWriter(fileStream))
                        writer.Write(bytes);
                }
                Fonts.AddFontFile(fullpath);
            }

        }
    }
}
