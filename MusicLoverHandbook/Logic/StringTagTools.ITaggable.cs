namespace MusicLoverHandbook.Logic
{
    public static partial class StringTagTools
    {
        public interface ITaggable
        {
            TagType Type { get; }
            TagDataType ValueType { get; }
            string? Value { get; }            
        }
    }
}