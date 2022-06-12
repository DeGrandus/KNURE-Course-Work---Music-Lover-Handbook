using MusicLoverHandbook.Models.Attributes;
using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum NoteType
    {
        [EnumColor(255, 0x6881DD)]
        Author,

        [EnumColor(255, 0x899FF0)]
        Disc,

        [EnumColor(255, 0x9DB0F3)]
        Song,

        [EnumColor(255, 0xAEBDF3)]
        SongFile,

        
        AddButton
    }

    static class Extensions
    {
        public static Color? GetColor(this NoteType value)
        {
            return
                value
                    .GetType()
                    .GetField(value.ToString())
                    ?.GetCustomAttribute<EnumColorAttribute>(false)
                    is EnumColorAttribute attr
              ? attr.Color
              : null;
        }
    }
}
