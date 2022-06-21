using MusicLoverHandbook.Models.Attributes;
using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum NoteType
    {
        [EnumColor(255, 0x6881DD)]
        [HasInputTypeEquvalence(InputType.Author)]
        Author,

        [EnumColor(255, 0x899FF0)]
        [HasInputTypeEquvalence(InputType.Disc)]
        Disc,

        [EnumColor(255, 0x9DB0F3)]
        [HasInputTypeEquvalence(InputType.SongName)]
        Song,

        [EnumColor(255, 0xAEBDF3)]
        [HasInputTypeEquvalence(InputType.SongFile)]
        SongFile,

        [EnumColor(255, 0xC9D3F7)]
        AddButton
    }

    class HasInputTypeEquvalenceAttribute : Attribute
    {
        public InputType Type { get; }

        public HasInputTypeEquvalenceAttribute(InputType type)
        {
            Type = type;
        }
    }

    static class Extensions
    {
        public static Color? GetColor(this NoteType value)
        {
            return value
                .GetType()
                .GetField(value.ToString())
                ?.GetCustomAttribute<EnumColorAttribute>(false)
                ?.Color;
        }

        public static InputType? AsInputType(this NoteType value)
        {
            return value
                .GetType()
                .GetField(value.ToString())
                ?.GetCustomAttribute<HasInputTypeEquvalenceAttribute>(false)
                ?.Type;
        }
    }
}
