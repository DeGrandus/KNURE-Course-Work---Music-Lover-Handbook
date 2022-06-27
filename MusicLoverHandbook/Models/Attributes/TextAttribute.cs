namespace MusicLoverHandbook.Models.Attributes
{
    public class TextAttribute : Attribute
    {
        public string Text { get; }

        public TextAttribute(string text)
        {
            Text = text;
        }
    }
}