using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteAdd : NoteControlChild
    {
        public override NoteType NoteType { get; } = NoteType.AddButton;

        public NoteAdd(NoteControlParent parent, string noteText, string noteDescription)
            : base(parent, noteText, noteDescription)
        {
            InitializeComponent();
            SideButtons.Deactivate(ButtonType.Delete);
            SideButtons.Deactivate(ButtonType.Edit);
            Icon = Properties.Resources.add;
            TextLabel.DoubleClick += (sender, e) => EditClick();
        }
        protected override LinkedList<SimpleNoteModel> GenerateNoteChain()
        {
            var chain = base.GenerateNoteChain();
            if (ParentNote is NoteControlParent parent)
            {
                InputType? newType = null;
                switch (parent.NoteType)
                {
                    case NoteType.Song:
                        newType = InputType.SongFile;
                        break;
                    case NoteType.Author:
                    case NoteType.Disc:
                        if (parent is NoteControlMidder midder)
                            if (midder.ParentNote is not NotesContainer)
                                newType = InputType.SongName;
                            else
                            if (parent.NoteType == NoteType.Disc)
                                newType = InputType.Author;
                            else
                                newType = InputType.Disc;
                        break;
                }
                
                if (newType == null) return chain;
                chain.AddLast(new SimpleNoteModel(,"",NoteType.AddButton));
            }
        }
    }
}
