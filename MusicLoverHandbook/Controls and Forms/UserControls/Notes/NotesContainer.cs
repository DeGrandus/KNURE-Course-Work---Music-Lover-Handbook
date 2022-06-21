using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public class NotesContainer : IControlParent
    {
        private List<INoteControlChild> partialInnerNotes = new();
        private bool useInnerNotes = true;
        public Panel PanelContainer { get; }
        public ObservableCollection<INoteControlChild> InnerNotes { get; }
        private List<INoteControlChild> PartialInnerNotes { get => useInnerNotes ? InnerNotes.ToList() : partialInnerNotes; }
        public QuickSearchController QSController { get; }

        public NotesContainer(Panel panelContainer, TextBox QSBar, BasicSwitchLabel QSSwitchLabel)
        {
            PanelContainer = panelContainer;
            InnerNotes = new ObservableCollection<INoteControlChild>();
            InnerNotes.CollectionChanged += OnHierarchyChanged;
            QSController = new(QSBar, this, QSSwitchLabel);
            QSController.ResultsChanged += (res) =>
            {
                if (res == null)
                {
                    partialInnerNotes.Clear();
                    useInnerNotes = true;
                }
                else
                {
                    partialInnerNotes = res;
                    useInnerNotes = false;
                }
                SetPartialToRender();
            };
        }

        private void SetPartialToRender()
        {
            PanelContainer.Controls.Clear();
            PanelContainer.Controls.AddRange(PartialInnerNotes.Where(x => x is Control).Cast<Control>().ToArray());
        }

        private void OnHierarchyChanged(
            object? sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e
        )
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                PanelContainer.Controls.Clear();
                return;
            }
            var newItems =
                e.NewItems
                    ?.Cast<INoteControlChild>()
                    .Where(x => x is NoteControl)
                    .Cast<NoteControl>()
                    .ToList() ?? new();
            var oldItems =
                e.OldItems
                    ?.Cast<INoteControlChild>()
                    .Where(x => x is NoteControl)
                    .Cast<NoteControl>()
                    .ToList() ?? new();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (newItems != null)
                        foreach (var item in newItems)
                        {
                            AddNote(item);
                        }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (oldItems != null)
                        foreach (var item in oldItems)
                            RemoveNote(item);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (oldItems != null && newItems != null)
                        foreach (
                            var item in Enumerable
                                .Zip(oldItems, newItems)
                                .Select(x => (Old: x.First, New: x.Second))
                        )
                        {
                            var ind = PanelContainer.Controls.IndexOf(item.Old);
                            RemoveNote(item.Old);
                            AddNote(item.New);
                        }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (newItems != null && newItems.Count == 1)
                    {
                        PanelContainer.Controls.SetChildIndex(newItems[0], e.NewStartingIndex);
                    }
                    break;
            }
        }

        private void AddNote(NoteControl note)
        {
            note.Dock = DockStyle.Top;
            RemoveNote(note);
            PanelContainer.Controls.Add(note);
            PanelContainer.Controls.SetChildIndex(note, 0);
            if (note is NoteControlParent asParent)
                SetupAddNoteButton(asParent);
        }

        private void RemoveNote(NoteControl note)
        {
            PanelContainer.Controls.Remove(note);
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

        public void SetupAddNoteButton(NoteControlParent note)
        {
            var potentialAdd = note.InnerNotes.ToList().Find(x => x.NoteType == NoteType.AddButton);
            if (potentialAdd?.NoteType is NoteType.AddButton)
                note.InnerNotes.Remove(potentialAdd);

            note.InnerNotes.Add(CreateAddButton(note));
            foreach (var inner in note.InnerNotes)
                if (inner is NoteControlParent innertParent)
                    SetupAddNoteButton(innertParent);
        }
    }
}
