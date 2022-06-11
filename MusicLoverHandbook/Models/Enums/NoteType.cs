using MusicLoverHandbook.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models.Enums
{
    public enum NoteType
    {
        [EnumColor(255, 0xC3A5D9)]
        Author,

        [EnumColor(255, 0xEFEDA3)]
        Disc,

        [EnumColor(255, 0xA9DCA8)]
        Song,

        [EnumColor(255, 0xCF91A8)]
        SongFile,

        [EnumColor(255, 0x759E8F)]
        AddButton
    }

    static class Extensions
    {
        public static Color GetColor(this NoteType value)
        {
            return
                value
                    .GetType()
                    .GetField(value.ToString())
                    ?.GetCustomAttribute<EnumColorAttribute>(false)
                    is EnumColorAttribute attr
              ? attr.Color
              : Color.White;
        }
    }
}
