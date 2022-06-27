using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Attributes;
using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum NoteType
    {
        [EnumColor(255, 0x6881DD, 255, 0xADB9DF)]
        [InformationCarrier]
        [ConnectedNoteType(typeof(NoteAuthor))]
        [ConnectedNoteCreationType(NoteCreationOrder.AuthorThenDisc)]
        Author,

        [EnumColor(255, 0x899FF0, 255, 0xAEDFDC)]
        [InformationCarrier]
        [ConnectedNoteType(typeof(NoteDisc))]
        [ConnectedNoteCreationType(NoteCreationOrder.DiscThenAuthor)]
        Disc,

        [EnumColor(255, 0x9DB0F3, 255, 0xE2ECAF)]
        [InformationCarrier]
        [StringValue("Song name")]
        [ConnectedNoteType(typeof(NoteSong))]
        Song,

        [EnumColor(255, 0xAEBDF3, 255, 0xC7B1D5)]
        [InformationCarrier]
        [StringValue("Song file")]
        [ConnectedNoteType(typeof(NoteSongFile))]
        SongFile,

        [EnumColor(255, 0xC9D3F7)]
        AddButton
    }

    public static partial class NoteTypeExtensions
    {
        public static Color? GetColor(this NoteType value)
        {
            return value
                .GetType()
                .GetField(value.ToString())
                ?.GetCustomAttribute<EnumColorAttribute>(false)
                ?.ColorMain;
        }

        public static Color? GetLiteColor(this NoteType value)
        {
            return value
                .GetType()
                .GetField(value.ToString())
                ?.GetCustomAttribute<EnumColorAttribute>(false)
                ?.ColorLite;
        }

        public static bool IsInformaionCarrier(this NoteType value)
        {
            return value
                    .GetType()
                    .GetField(value.ToString())!
                    .GetCustomAttribute<InformationCarrierAttribute>() != null;
        }
    }

    public static partial class NoteTypeExtensions
    {
        public static NoteCreationOrder? GetConnectedCreationOrder(this NoteType type)
        {
            return type.GetType()
                .GetField(type.ToString())
                ?.GetCustomAttribute<ConnectedNoteCreationTypeAttribute>()
                ?.Type;
        }

        public static Type? GetConnectedNoteType(this NoteType type)
        {
            return type.GetType()
                .GetField(type.ToString())
                ?.GetCustomAttribute<ConnectedNoteTypeAttribute>()
                ?.ConnectedType;
        }

        public static string ToString(this Enum type, bool useCustomStringValue)
        {
            if (useCustomStringValue)
                return type.GetType()
                        .GetField(type.ToString())
                        ?.GetCustomAttribute<StringValueAttribute>()
                        ?.Value ?? type.ToString();
            return type.ToString();
        }
    }

    public class ConnectedNoteCreationTypeAttribute : Attribute
    {
        public NoteCreationOrder Type { get; }

        public ConnectedNoteCreationTypeAttribute(NoteCreationOrder type)
        {
            Type = type;
        }
    }

    public class ConnectedNoteTypeAttribute : Attribute
    {
        public Type ConnectedType { get; }

        public ConnectedNoteTypeAttribute(Type noteType)
        {
            ConnectedType = noteType;
        }
    }

    public class InformationCarrierAttribute : Attribute { }

    public class StringValueAttribute : Attribute
    {
        public string Value { get; }

        public StringValueAttribute(string value)
        {
            Value = value;
        }
    }
}