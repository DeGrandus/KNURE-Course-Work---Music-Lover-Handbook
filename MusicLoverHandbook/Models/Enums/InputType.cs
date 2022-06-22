using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum InputType
    {
        [ConnectedNoteType(typeof(NoteAuthor))]
        [ConnectedNoteCreationType(NoteCreationOrder.AuthorThenDisc)]
        Author,

        [ConnectedNoteType(typeof(NoteDisc))]
        [ConnectedNoteCreationType(NoteCreationOrder.DiscThenAuthor)]
        Disc,

        [ConnectedNoteType(typeof(NoteSong))]
        [StringValue("Song name")]
        SongName,

        [ConnectedNoteType(typeof(NoteSongFile))]
        [StringValue("Song file")]
        SongFile,
    }

    public static class InputTypeExtensions
    {
        public static NoteCreationOrder? GetConnectedCreationType(this InputType type)
        {
            return type.GetType()
                .GetField(type.ToString())
                ?.GetCustomAttribute<ConnectedNoteCreationTypeAttribute>()
                ?.Type;
        }

        public static Type? GetConnectedNoteType(this InputType type)
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