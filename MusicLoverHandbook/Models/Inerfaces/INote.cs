using MusicLoverHandbook.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INote
    {
        string NoteText { get; set; }
        string NoteDescription { get; set; }
    }
    public interface INoteParent<InnerNotesType> where InnerNotesType : INote
    { 
        public IReadOnlyCollection<InnerNotesType> InnerNotes { get; }

    }
    public interface INoteChild<ParentNoteType> where ParentNoteType : INote
    {
        public ParentNoteType ParentNote { get; }

    }
    public interface INoteMidder<ParentNoteType, InnerNotesType> : INoteChild<ParentNoteType>, INoteParent<InnerNotesType> where ParentNoteType : INote where InnerNotesType : INote
    {

    }
}
