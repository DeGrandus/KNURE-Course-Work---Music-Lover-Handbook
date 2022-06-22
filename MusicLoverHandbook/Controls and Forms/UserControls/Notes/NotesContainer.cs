using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
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

        public NotesContainer(Panel panelContainer, TextBox QSBar, BasicSwitchLabel QSSwitchLabel)
        {
            PanelContainer = panelContainer;
            InnerNotes = new ObservableCollection<INoteControlChild>();
            InnerNotes.CollectionChanged += OnHierarchyChanged;

            Task.Run(
                () =>
                {
                    while (true)
                        if (panelContainer.FindForm() != null)
                            panelContainer.FindForm().Text = PartialInnerNotes.Count.ToString();
                }
            );
            QSController = new(QSBar, this, QSSwitchLabel);
            QSController.ResultsChanged += (res) =>
            {
                if (res == null)
                    PartialInnerNotes = InnerNotes.ToList();
                else
                    PartialInnerNotes = res;
            };
        }

        public ObservableCollection<INoteControlChild> InnerNotes { get; }
        public Panel PanelContainer { get; }

        public List<INoteControlChild> PartialInnerNotes
        {
            get => partialInnerNotes;
            private set
            {
                partialInnerNotes = value;
                RefreshRender();
            }
        }

        public QuickSearchController QSController { get; }

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

        private void AddNote(NoteControl note)
        {
            note.Dock = DockStyle.Top;
            RemoveNote(note);
            PanelContainer.Controls.Add(note);
            PanelContainer.Controls.SetChildIndex(note, 0);
            if (note is NoteControlParent asParent)
                SetupAddNoteButton(asParent);
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

        private void RemoveNote(NoteControl note)
        {
            PanelContainer.Controls.Remove(note);
        }

        private void RefreshRender()
        {
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
        }
    }
}
