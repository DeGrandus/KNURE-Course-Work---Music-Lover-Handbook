namespace MusicLoverHandbook.Models
{
    public class ColorThemeEventArgs : EventArgs
    {
        public ColorThemeEventArgs(Color color)
        {
            Color = color;
        }

        public Color Color { get; }
    }
}