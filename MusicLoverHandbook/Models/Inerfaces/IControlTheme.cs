namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface IControlTheme
    {
        public delegate void ThemeChangeEventHandler(object? sender, ColorThemeEventArgs e);

        public event ThemeChangeEventHandler ColorChanged;

        Color ThemeColor { get; set; }
    }
}
