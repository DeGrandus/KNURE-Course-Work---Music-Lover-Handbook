namespace MusicLoverHandbook.Models.Attributes
{
    public class TextAttribute : Attribute
    {
        #region Public Properties

        public string Text { get; }

        #endregion Public Properties

        #region Public Constructors + Destructors

        public TextAttribute(string text)
        {
            Text = text;
        }

        #endregion Public Constructors + Destructors
    }
}
