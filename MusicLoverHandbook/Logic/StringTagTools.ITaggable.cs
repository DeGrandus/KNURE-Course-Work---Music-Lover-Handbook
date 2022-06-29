namespace MusicLoverHandbook.Logic
{
    public static partial class StringTagTools
    {
        #region Public Interfaces

        public interface ITaggable
        {
            #region Public Properties

            TagType Type { get; }
            string? Value { get; }
            TagDataType ValueType { get; }

            #endregion Public Properties
        }

        #endregion Public Interfaces
    }
}