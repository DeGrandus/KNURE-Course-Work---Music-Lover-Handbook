using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Attributes;
using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum NoteType
    {
        [EnumColor(255, 0x6881DD)]
        [HasInputTypeEquvalence(NoteType.Author)]
        [InformationCarrier]
        [ConnectedNoteType(typeof(NoteAuthor))]
        [ConnectedNoteCreationType(NoteCreationOrder.AuthorThenDisc)]
        Author,

        [EnumColor(255, 0x899FF0)]
        [HasInputTypeEquvalence(NoteType.Disc)]
        [InformationCarrier]
        [ConnectedNoteType(typeof(NoteDisc))]
        [ConnectedNoteCreationType(NoteCreationOrder.DiscThenAuthor)]
        Disc,

        [EnumColor(255, 0x9DB0F3)]
        [HasInputTypeEquvalence(NoteType.Song)]
        [InformationCarrier]
        [StringValue("Song name")]
        [ConnectedNoteType(typeof(NoteSong))]
        Song,

        [EnumColor(255, 0xAEBDF3)]
        [HasInputTypeEquvalence(NoteType.SongFile)]
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
        public HasInputTypeEquvalenceAttribute(NoteType type)
        {
            Type = type;
        }

        public NoteType Type { get; }
    }
    public static partial class NoteTypeExtensions
    {
        public static NoteCreationOrder? GetConnectedCreationType(this NoteType type)
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
        public ConnectedNoteCreationTypeAttribute(NoteCreationOrder type)
        {
            Type = type;
        }

        public NoteCreationOrder Type { get; }
    }

    public class ConnectedNoteTypeAttribute : Attribute
    {
        public ConnectedNoteTypeAttribute(Type noteType)
        {
            ConnectedType = noteType;
        }

        public Type ConnectedType { get; }
    }

    public class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}