namespace MusicLoverHandbook.Logic
{
    public static partial class StringTagTools
    {
        #region Public Classes

        public class TagName : Taggable
        {
            #region Public Properties

            public override TagType Type { get; }

            #endregion Public Properties

            #region Public Constructors

            public TagName(TagDataType type, string? value = null) : base(type, value)
            {
                Type = TagType.Name;
            }

            #endregion Public Constructors
        }

        #endregion Public Classes
    }
}