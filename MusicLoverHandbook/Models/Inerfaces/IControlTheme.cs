namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface IControlTheme
    {
        Color ThemeColor { get; set; }
        public event ThemeChangeEventHandler ColorChanged;
        public delegate void ThemeChangeEventHandler(object? sender, ColorThemeEventArgs e);
    }
}
