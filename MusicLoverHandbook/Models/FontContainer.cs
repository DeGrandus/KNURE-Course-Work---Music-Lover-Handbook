using System.Drawing.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MusicLoverHandbook.Models
{
    public class FontContainer
    {
        static FontContainer()
        {
            Instance = new FontContainer();
        }

        public FontContainer()
        {
            var temppath = Path.GetTempPath();
            var executing = Assembly.GetExecutingAssembly();
            var fonts = executing
                .GetManifestResourceNames()
                .Where(x => Regex.IsMatch(x, @"\.tff|\.otf"))
                .ToList();
            Families = new FontFamily[fonts.Count()];
            for (var i = 0; i < fonts.Count; i++)
            {
                FontFamily output = FontFamily.GenericSansSerif;
                try
                {
                    try
                    {
                        output = new FontFamily("Mariupol");
                    }
                    catch (ArgumentException)
                    {
                        var font = fonts[i];
                        var fullpath = Path.Combine(temppath, font);
                        if (!File.Exists(fullpath))
                        {
                            var stream = executing.GetManifestResourceStream(font);
                            if (stream == null)
                                continue;

                            var bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, (int)bytes.Length);
                            stream.Close();

                            using (var fileStream = File.OpenWrite(fullpath))
                            using (var writer = new BinaryWriter(fileStream))
                                writer.Write(bytes);
                        }
                        var coll = new PrivateFontCollection();
                        coll.AddFontFile(fullpath);
                        output = coll.Families[0];
                    }
                }
                catch
                {
                    try
                    {
                        output = new FontFamily("Segoe UI");
                    }
                    catch { }
                }
                finally
                {
                    Families[i] = output;
                }
                ;
            }
        }

        public static FontContainer Instance { get; }
        public FontFamily[] Families { get; }
    }
}
