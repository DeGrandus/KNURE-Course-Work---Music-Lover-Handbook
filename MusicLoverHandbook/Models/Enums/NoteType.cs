using MusicLoverHandbook.Models.Attributes;
using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum NoteType
    {
        [EnumColor(255, 0x6881DD)]
        [HasInputTypeEquvalence(InputType.Author)]
        [InformationCarrier]
        Author,

        [EnumColor(255, 0x899FF0)]
        [HasInputTypeEquvalence(InputType.Disc)]
        [InformationCarrier]
        Disc,

        [EnumColor(255, 0x9DB0F3)]
        [HasInputTypeEquvalence(InputType.SongName)]
        [InformationCarrier]
        [StringValue("Song name")]
        Song,

        [EnumColor(255, 0xAEBDF3)]
        [HasInputTypeEquvalence(InputType.SongFile)]
        [InformationCarrier]
        [StringValue("Song file")]
        SongFile,

        [EnumColor(255, 0xC9D3F7)]
        AddButton
    }

    public static class NoteTypeExtensions
    {
        public static InputType? AsInputType(this NoteType value)
        {
            return value
                .GetType()
                .GetField(value.ToString())
                ?.GetCustomAttribute<HasInputTypeEquvalenceAttribute>(false)
                ?.Type;
        }

        public static Color? GetColor(this NoteType value)
        {
            return value
                .GetType()
                .GetField(value.ToString())
                ?.GetCustomAttribute<EnumColorAttribute>(false)
                ?.Color;
        }
        public static bool IsInformaionCarrier(this NoteType value)
        {
            return value.GetType().GetField(value.ToString())!.GetCustomAttribute<InformationCarrierAttribute>() != null;
        }
    }
    public class InformationCarrierAttribute : Attribute
    {

    }
    internal class HasInputTypeEquvalenceAttribute : Attribute
    {
        public HasInputTypeEquvalenceAttribute(InputType type)
        {
            Type = type;
        }

        public InputType Type { get; }
    }
}