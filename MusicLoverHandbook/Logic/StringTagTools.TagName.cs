namespace MusicLoverHandbook.Logic
{
    public static partial class StringTagTools
    {
        public class TagName : Taggable
        {
            public override TagType Type { get; }
            public TagName(TagDataType type, string? value = null) : base(type,value)
            {
                Type = TagType.Name;
            }
        }
    }
}