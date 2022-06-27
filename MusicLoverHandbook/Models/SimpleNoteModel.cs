using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models
{
    public class SimpleNoteModel
    {
        public string Description { get; }

        public string Name { get; }

        public NoteType NoteType { get; }

        public SimpleNoteModel(INoteControl note)
        {
            NoteType = note.NoteType;
            Name = note.NoteName;
            Description = note.NoteDescription;
        }

        public SimpleNoteModel(string name, string description, NoteType noteType)
        {
            Name = name;
            Description = description;
            NoteType = noteType;
        }
    }
}