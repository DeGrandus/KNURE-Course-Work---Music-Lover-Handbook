namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface IColorThemeSupported
    {
        Color ThemeColor { get; set; }

        public delegate void ThemeChangeEventHandler(object? sender, ColorThemeEventArgs e);

        public event ThemeChangeEventHandler ColorChanged;
    }
}
