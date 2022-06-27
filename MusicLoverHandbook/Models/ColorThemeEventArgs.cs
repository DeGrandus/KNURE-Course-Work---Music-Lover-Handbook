namespace MusicLoverHandbook.Models
{
    public class ColorThemeEventArgs : EventArgs
    {
        public Color Color { get; }

        public ColorThemeEventArgs(Color color)
        {
            Color = color;
        }
    }
}