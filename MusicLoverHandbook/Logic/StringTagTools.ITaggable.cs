namespace MusicLoverHandbook.Logic
{
    public static partial class StringTagTools
    {
        public interface ITaggable
        {
            TagType Type { get; }
            string? Value { get; }
            TagDataType ValueType { get; }
        }
    }
}