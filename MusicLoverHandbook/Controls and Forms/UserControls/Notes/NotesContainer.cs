using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Timer = System.Windows.Forms.Timer;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public class NotesContainer : IControlParent
    {
        private List<INoteControlChild> partialInnerNotes = new();

        public NotesContainer(Panel panelContainer, TextBox QSBar, BasicSwitchLabel QSSwitchLabel)
        {
            PanelContainer = panelContainer;

            InnerNotes = new ObservableCollection<INoteControlChild>();
            InnerNotes.CollectionChanged += OnHierarchyChanged;

            QSController = new(QSBar, this, QSSwitchLabel);
            QSController.ResultsChanged += (res) =>
            {
                if (res == null)
                    PartialInnerNotes = InnerNotes.ToList();
                else
                    PartialInnerNotes = res;
            };

            renderRefreshTimer = new Timer()
            {
                Interval = 1,
                Enabled = true,
            };
            renderRefreshTimer.Tick += (sender, e) =>
            {
                if (refreshDelay >= 0)
                {
                    if (refreshDelay == 0)
                        RefreshNoteContainer();
                    refreshDelay--;
                }
            };
        }

        private Timer renderRefreshTimer;
        public ObservableCollection<INoteControlChild> InnerNotes { get; }
        public Panel PanelContainer { get; }

        public List<INoteControlChild> PartialInnerNotes
        {
            get => partialInnerNotes;
            private set
            {
                partialInnerNotes = value;
                RefreshNoteContainerDelayed();
            }
        }

        public QuickSearchController QSController { get; }

        public void SetupAddNoteButton(NoteControlParent note)
        {
            PanelContainer.SuspendLayout();
            var potentialAdd = note.InnerNotes.ToList().Find(x => x.NoteType == NoteType.AddButton);
            if (potentialAdd?.NoteType is NoteType.AddButton)
                note.InnerNotes.Remove(potentialAdd);
            var add = CreateAddButton(note);
            note.InnerNotes.Add(add);
            note.InnerContentPanel.Controls.SetChildIndex(add,0);
            foreach (var inner in note.InnerNotes)
                if (inner is NoteControlParent innertParent)
                    SetupAddNoteButton(innertParent);
            PanelContainer.ResumeLayout();

        }

        private NoteAdd CreateAddButton(NoteControlParent parent)
        {
            NoteType next;
            if (parent is NoteAuthor author)
                if (author.ParentNote is NoteDisc)
                    next = NoteType.Song;
                else

                    next = NoteType.Disc;
            else if (parent is NoteDisc disc)
                if (disc.ParentNote is NoteAuthor)
                    next = NoteType.Song;
                else

                    next = NoteType.Author;
            else
                next = parent.NoteType + 1;
            return new NoteAdd(parent, $"Add new {next}", "Click on me to add new note");
        }

        private void OnHierarchyChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PartialInnerNotes = InnerNotes.ToList();
            return;
        }
        private int refreshDelay = 0;
        private void RefreshNoteContainerDelayed()
        {
            refreshDelay = 20;
        }
        private void RefreshNoteContainer()
        {
            PanelContainer.SuspendLayout();
            PanelContainer.Controls.Clear();
            var renderFinal = PartialInnerNotes;

            foreach (var child in renderFinal)
            {
                if (child is Control ctrl)
                    ctrl.Dock = DockStyle.Top;
                if (child is NoteControlParent asParent)
                    SetupAddNoteButton(asParent);
            }
            //MAY BE SOME ACTIONS ON RENDER FINAL FROM SORT


            PanelContainer.Controls.AddRange(
                renderFinal
                    .Reverse<INoteControlChild>()
                    .Where(x => x is Control)
                    .Cast<Control>()
                    .ToArray()
            );
            PanelContainer.ResumeLayout();
        }
    }
}
