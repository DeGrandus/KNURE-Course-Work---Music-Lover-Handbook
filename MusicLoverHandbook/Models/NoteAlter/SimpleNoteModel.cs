using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models.NoteAlter
{
    public class SimpleNoteModel
    {
        #region Public Properties

        public string Description { get; }

        public string Name { get; }

        public NoteType NoteType { get; }

        #endregion Public Properties

        #region Public Constructors + Destructors

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

        #endregion Public Constructors + Destructors
    }
}
