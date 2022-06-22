﻿using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
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
        public NoteCreationOrder Type { get; }

        public ConnectedNoteCreationTypeAttribute(NoteCreationOrder type)
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
            return type.GetType()
                .GetField(type.ToString())
                ?.GetCustomAttribute<ConnectedNoteTypeAttribute>()
                ?.ConnectedType;
        }

        public static NoteCreationOrder? GetConnectedCreationType(this InputType type)
        {
            return type.GetType()
                .GetField(type.ToString())
                ?.GetCustomAttribute<ConnectedNoteCreationTypeAttribute>()
                ?.Type;
        }

        public static string ToString(this InputType type, bool useCustomStringValue)
        {
            if (useCustomStringValue)
                return type.GetType()
                        .GetField(type.ToString())
                        ?.GetCustomAttribute<StringValueAttribute>()
                        ?.Value ?? type.ToString();
            return type.ToString();
        }
    }
}
