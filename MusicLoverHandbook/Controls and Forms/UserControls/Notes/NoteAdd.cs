using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;

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
                NoteType? newType = null;
                string descriptionExample = "Description example";
                switch (parent.NoteType)
                {
                    case NoteType.Song:
                        newType = NoteType.SongFile;
                        descriptionExample =
                            "Full path to the mp3 file (not neccesary)"
                            + "\r\n"
                            + descriptionExample;
                        break;
                    case NoteType.Author:
                    case NoteType.Disc:
                        if (parent is NoteControlMidder midder)
                            if (midder.ParentNote is not NotesContainer)
                                newType = NoteType.Song;
                            else if (parent.NoteType == NoteType.Disc)
                                newType = NoteType.Author;
                            else
                                newType = NoteType.Disc;
                        break;
                }

                if (newType == null)
                    return chain;
                chain.AddLast(
                    new SimpleNoteModel(
                        newType.Value.AsInputType()?.ToString(true) ?? "",
                        descriptionExample,
                        newType.Value
                    )
                );
            }
            return chain;
        }
    }
}
