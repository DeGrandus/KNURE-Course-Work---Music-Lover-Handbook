using System.Drawing.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MusicLoverHandbook.Models
{
    public class FontContainer
    {
        public static FontContainer Instance { get; }

        public FontFamily[] LoadedFamilies { get; }

        static FontContainer()
        {
            Instance = new FontContainer();
        }

        public FontContainer()
        {
            var tempPath = Path.GetTempPath();
            var executingAssembly = Assembly.GetExecutingAssembly();
            var fonts = executingAssembly
                .GetManifestResourceNames()
                .Where(x => Regex.IsMatch(x, @"\.tff|\.otf"))
                .ToList();
            LoadedFamilies = new FontFamily[fonts.Count()];
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
                        var fullpath = Path.Combine(tempPath, font);
                        if (!File.Exists(fullpath))
                        {
                            var stream = executingAssembly.GetManifestResourceStream(font);
                            if (stream == null)
                                continue;

                            var bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, bytes.Length);
                            stream.Close();

                            using (var fileStream = File.OpenWrite(fullpath))
                            using (var writer = new BinaryWriter(fileStream))
                                writer.Write(bytes);
                        }
                        var pfc = new PrivateFontCollection();
                        pfc.AddFontFile(fullpath);
                        output = pfc.Families[0];
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
                    LoadedFamilies[i] = output;
                }
                ;
            }
        }
    }
}
