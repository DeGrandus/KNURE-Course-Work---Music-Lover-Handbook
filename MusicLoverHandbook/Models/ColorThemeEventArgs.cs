namespace MusicLoverHandbook.Models
{
    public class ColorThemeEventArgs : EventArgs
    {
        #region Public Properties

        public Color Color { get; }

        #endregion Public Properties

        #region Public Constructors

        public ColorThemeEventArgs(Color color)
        {
            Color = color;
        }

        #endregion Public Constructors
    }
}