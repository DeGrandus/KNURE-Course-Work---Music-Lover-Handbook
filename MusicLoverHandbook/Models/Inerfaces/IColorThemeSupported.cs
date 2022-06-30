namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface IColorThemeSupported : INoteControl
    {
        #region Public Properties

        Color ThemeColor { get; set; }

        #endregion Public Properties

        #region Public Delegates

        public delegate void ThemeChangeEventHandler(object? sender, ColorThemeEventArgs e);

        #endregion Public Delegates

        #region Public Events

        public event ThemeChangeEventHandler ColorChanged;

        #endregion Public Events
    }
}
