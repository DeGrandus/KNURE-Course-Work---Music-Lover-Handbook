namespace MusicLoverHandbook.Logic
{
    public static partial class StringTagTools
    {
        public class TagValue : Taggable
        {
            public override TagType Type { get; }

            public TagValue(TagDataType type, string? value = null) : base(type, value)
            {
                Type = TagType.Value;
            }
        }
    }
}