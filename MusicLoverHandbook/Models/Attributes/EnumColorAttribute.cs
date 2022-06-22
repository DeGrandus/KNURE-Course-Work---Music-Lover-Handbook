namespace MusicLoverHandbook.Models.Attributes
{
    public class EnumColorAttribute : Attribute
    {
        public bool Inheritance = false;

        public EnumColorAttribute(int alpha, int color)
        {
            Color = Color.FromArgb(alpha, Color.FromArgb(color));
        }

        public EnumColorAttribute(bool inherit)
        {
            Inheritance = inherit;
        }

        public Color Color { get; }
    }
}