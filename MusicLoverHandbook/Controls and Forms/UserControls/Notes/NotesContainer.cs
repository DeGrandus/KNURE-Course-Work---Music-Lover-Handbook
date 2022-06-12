using MusicLoverHandbook.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public class NotesContainer
    {
        public Panel PanelContainer { get; }
        public List<NoteControl> Hierarchy { get; }

        public NotesContainer(Panel panelContainer)
        {
            PanelContainer = panelContainer;
            Hierarchy = new List<NoteControl>();
        }

        public void AddNote(NoteControlParent note)
        {
            note.Dock = DockStyle.Top;
            Hierarchy.Add(note);
            PanelContainer.Controls.Add(note);
            PanelContainer.Controls.SetChildIndex(note, 0);
            SetupAddNoteButton(note);
        }

        public void DeleteNote(NoteControlParent note)
        {
            Hierarchy.Remove(note);
            PanelContainer.Controls.Remove(note);
        }

        private NoteAdd CreateAddButton(NoteControlParent parent) =>
            new NoteAdd(parent, $"Add new {parent.NoteType + 1}", "Click on me to add new note");

        private void SetupAddNoteButton(NoteControlParent note) =>
            SetupAddNoteButton(note, true, true);

        private void SetupAddNoteButton(
            NoteControlParent note,
            bool recursiveForParent,
            bool recursiveForChildren
        )
        {
            if (!note.InnerNotes.Select(x => x.NoteType).Contains(Models.Enums.NoteType.AddButton))
                note.InnerNotes.Add(CreateAddButton(note));
            if (recursiveForChildren)
                foreach (var child in note.InnerNotes)
                {
                    if (child is NoteControlParent anotherInnerParent)
                        SetupAddNoteButton(anotherInnerParent, false, true);
                }
            if (recursiveForParent)
                if (
                    note is NoteControlMidder midder
                    && midder.ParentNote is NoteControlParent anotherParent
                )
                    SetupAddNoteButton(anotherParent, true, false);
        }
    }
}
