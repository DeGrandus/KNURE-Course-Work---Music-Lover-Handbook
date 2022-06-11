namespace MusicLoverHandbook.Models.Attributes
{
    public class EnumColorAttribute : Attribute
    {
        public Color Color { get; }

        public EnumColorAttribute(int alpha, int color)
        {
            Color = Color.FromArgb(alpha, Color.FromArgb(color));
        }
    }
}
