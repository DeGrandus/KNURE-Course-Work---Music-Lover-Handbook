using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models
{
    public class NoteControl : UserControl, INote
    {
        public Image? Icon { get; set; }
        public int Offset { get; set; } = 10;
        public int TrueOffset { get; set; }
        public NoteControlOffsetType OffsetType { get; set; } = NoteControlOffsetType.Relative;
        public Color StripColor { get; set; }
        public string NoteText { get; set; }
        public string NoteDescription { get; set; }
        protected NoteControl(Color stripColor, string text, string description)
        {
            StripColor = stripColor;
            NoteText = text;
            NoteDescription = description;
        }
    }
    public class NoteControlChild<ParentNoteType> : NoteControl, INoteChild<ParentNoteType> where ParentNoteType : NoteControl
    {
        public ParentNoteType ParentNote { get; set; }
        ParentNoteType INoteChild<ParentNoteType>.ParentNote => ParentNote;
        protected NoteControlChild(ParentNoteType parent,Color stripColor, string text, string description) : base(stripColor, text, description)
        {
            ParentNote = parent;
        } 
    }
    public class NoteControlParent<InnerNotesType> : NoteControl, INoteParent<InnerNotesType> where InnerNotesType : NoteControl
    {
        public ObservableCollection<InnerNotesType> InnerNotes { get; set; } = new();
        IReadOnlyCollection<InnerNotesType> INoteParent<InnerNotesType>.InnerNotes => InnerNotes.ToList();

        protected NoteControlParent(Color stripColor, string text, string description) : base(stripColor, text, description)
        {

        }
    }
    public class NoteControlMidder<ParentNoteType,InnerNotesType> : NoteControl,INoteMidder<ParentNoteType,InnerNotesType> where ParentNoteType:NoteControl where InnerNotesType : NoteControl
    {
        public ParentNoteType ParentNote { get; set; }
        ParentNoteType INoteChild<ParentNoteType>.ParentNote => ParentNote;
        public ObservableCollection<InnerNotesType> InnerNotes { get; set; } = new();
        IReadOnlyCollection<InnerNotesType> INoteParent<InnerNotesType>.InnerNotes => InnerNotes.ToList();

        protected NoteControlMidder(ParentNoteType parent, Color stripColor, string text, string description) : base(stripColor, text, description)
        {
            ParentNote = parent;
        }
    }

}
