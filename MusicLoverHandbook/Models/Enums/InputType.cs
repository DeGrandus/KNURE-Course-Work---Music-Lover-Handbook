using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum InputType
    {
        [ConnectedNoteType(typeof(NoteAuthor))]
        [ConnectedNoteCreationType(NoteCreationType.DiscInAuthor)]
        Author,
        [ConnectedNoteType(typeof(NoteDisc))]
        [ConnectedNoteCreationType(NoteCreationType.AuthorInDisc)]
        Disc,
        [ConnectedNoteType(typeof(NoteSong))]
        [StringValue("Name of song")]
        SongName,
        [ConnectedNoteType(typeof(NoteSongFile))]
        [StringValue("Song file")]
        SongFile,
    }
    public class ConnectedNoteTypeAttribute : Attribute
    {
        public Type ConnectedType { get; }
        public ConnectedNoteTypeAttribute(Type noteType)
        {
            ConnectedType = noteType;
        }
    }
    public class ConnectedNoteCreationTypeAttribute : Attribute
    {
        public NoteCreationType Type { get; }
        public ConnectedNoteCreationTypeAttribute(NoteCreationType type)
        {
            Type = type;
        }
    }
    public class StringValueAttribute : Attribute
    {
        public string Value { get; }
        public StringValueAttribute(string value)
        {
            Value = value;
        }
    }
    public static class InputTypeExt
    {
        public static Type? GetConnectedNoteType(this InputType type)
        {
            return type.GetType().GetField(type.ToString())?.GetCustomAttribute<ConnectedNoteTypeAttribute>()?.ConnectedType;
        }
        public static NoteCreationType? GetConnectedCreationType(this InputType type)
        {
            return type.GetType().GetField(type.ToString())?.GetCustomAttribute<ConnectedNoteCreationTypeAttribute>()?.Type;
        }
        public static string ToString(this InputType type)
        {
            return type.GetType().GetField(Enum.GetName(type) ?? "")?.GetCustomAttribute<StringValueAttribute>()?.Value??Enum.GetName(type)??"";
        }
    }
}
