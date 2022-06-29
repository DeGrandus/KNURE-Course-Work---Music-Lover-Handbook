namespace MusicLoverHandbook.Logic
{
    public static partial class StringTagTools
    {
        #region Public Classes

        public class TagValue : Taggable
        {
            #region Public Properties

            public override TagType Type { get; }

            #endregion Public Properties

            #region Public Constructors

            public TagValue(TagDataType type, string? value = null) : base(type, value)
            {
                Type = TagType.Value;
            }

            #endregion Public Constructors
        }

        #endregion Public Classes
    }
}
