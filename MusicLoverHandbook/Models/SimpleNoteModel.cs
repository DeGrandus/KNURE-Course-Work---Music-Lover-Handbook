using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models
{
    public class SimpleNoteModel
    {
        public string Name { get; }
        public string Description { get; }
        public NoteType NoteType { get; }
        public SimpleNoteModel(INoteControl note)
        {
            NoteType = note.NoteType;
            Name = note.NoteName;
            Description = note.NoteDescription;
        }
    }
}
