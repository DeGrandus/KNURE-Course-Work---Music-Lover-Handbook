using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum InputType
    {
        [ConnectedNoteType(typeof(NoteAuthor))]
        Author,
        [ConnectedNoteType(typeof(NoteDisc))]
        Disc,
        [ConnectedNoteType(typeof(NoteSong))]
        SongName,
        [ConnectedNoteType(typeof(NoteSongFile))]
        SongFile,
    }
    public class ConnectedNoteTypeAttribute : Attribute
    {
        public Type ConnectedType { get; set; }
        public ConnectedNoteTypeAttribute(Type noteType)
        {
            ConnectedType = noteType;
        }
    }
    public static class InputTypeExt
    {
        public static Type? GetConnectedNoteType(this InputType type)
        {
            return type.GetType().GetField(type.ToString())?.GetCustomAttribute<ConnectedNoteTypeAttribute>()?.ConnectedType;
        }
    }
}
