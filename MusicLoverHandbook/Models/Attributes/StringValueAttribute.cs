namespace MusicLoverHandbook.Models.Attributes
{
    public class StringValueAttribute : Attribute
    {
        #region Public Properties

        public string Value { get; }

        #endregion Public Properties



        #region Public Constructors

        public StringValueAttribute(string value)
        {
            Value = value;
        }

        #endregion Public Constructors
    }
}
