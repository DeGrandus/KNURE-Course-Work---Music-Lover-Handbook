namespace MusicLoverHandbook.Models.Attributes
{
    public class TextAttribute : Attribute
    {
        public TextAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}