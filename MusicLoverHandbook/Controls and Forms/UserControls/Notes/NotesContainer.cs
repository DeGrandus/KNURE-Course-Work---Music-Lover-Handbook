using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public class NotesContainer : IParentControl
    {
        private List<INoteControlChild> partialInnerNotes = new();
        private TextBox qSTextBox;
        public NotesContainer(Panel panelContainer, TextBox QSTextBox, BasicSwitchLabel QSSwitchLabel)
        {
            PanelContainer = panelContainer;

            InnerNotes = new ObservableCollection<INoteControlChild>();

            QSController = new(this);
            QSController.ResultsChanged += (result) =>
            {
                Debug.WriteLine($"QSResults: {result.Count()}");
                PartialInnerNotes = result.ToList();
            };
            qSTextBox = QSTextBox;
            QSTextBox.TextChanged += (sender, e) =>
            
                InvokeQuickSearch();
            
            QSSwitchLabel.SpecialStateChanged += (sender, state) =>
            {
                QSController.IsDescriptionIncluded = state;
                InvokeQuickSearch();
            };
            InnerNotes.CollectionChanged+= (sender, e) =>
            {
                Debug.WriteLine($"Container logging: changing collection");
                Debug.WriteLine($"Container logging: InnerNotes count - {InnerNotes.Count}");
                Debug.WriteLine($"Container logging: CurrentNotes count - {CurrentlyActiveNotes.Count}");
                InvokeQuickSearch();
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
                        ContainmentRefreshing();
                    refreshDelay--;
                }
            };
        }
        public void InvokeQuickSearch() => QSController.InvokeQuickSearch(qSTextBox.Text);
        public List<INoteControlChild> CurrentlyActiveNotes => AdvancedFilteredNotes ?? InnerNotes.ToList();

        private Timer renderRefreshTimer;
        public ObservableCollection<INoteControlChild> InnerNotes { get; }
        public Panel PanelContainer { get; }

        public List<INoteControlChild> PartialInnerNotes
        {
            get => partialInnerNotes;
            private set
            {
                partialInnerNotes = value;
                DelayedContainmentRefreshing();
            }
        }

        public QuickSearchController QSController { get; }
        public List<Func<IEnumerable<INoteControlChild>, IEnumerable<INoteControlChild>>> Filters
        {
            get => filters; set
            {
                filters = value;
                ContainmentRefreshing();
            }
        }
        public List<INoteControlChild>? AdvancedFilteredNotes
        {
            get => advancedFilteredNotes; set
            {
                advancedFilteredNotes = value;
            }
        }

        public void SetupAddNoteButton(NoteControlParent note)
        {
            PanelContainer.SuspendLayout();
            var potentialAdd = note.InnerNotes.ToList().Find(x => x.NoteType == NoteType.AddButton);
            if (potentialAdd?.NoteType is NoteType.AddButton)
                note.InnerNotes.Remove(potentialAdd);
            var add = CreateAddButton(note);
            note.InnerNotes.Add(add);
            note.InnerContentPanel.Controls.SetChildIndex(add, 0);
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
        private int refreshDelay = 0;
        private void DelayedContainmentRefreshing()
        {
            refreshDelay = 20;
        }
        private List<Func<IEnumerable<INoteControlChild>, IEnumerable<INoteControlChild>>> filters = new();
        private List<INoteControlChild>? advancedFilteredNotes;

        private IEnumerable<INoteControlChild> RecursiveFiltering(IEnumerable<INoteControlChild> notes, Func<IEnumerable<INoteControlChild>, IEnumerable<INoteControlChild>> filteringFunc)
        {
            var output = filteringFunc(notes);
            foreach (var note in notes)
                if (note is INoteControlParent asParent)
                    asParent.InnerNotes = new(filteringFunc(asParent.InnerNotes));
            return output;
        }
        private void ContainmentRefreshing()
        {
            PanelContainer.SuspendLayout();

            PanelContainer.Controls.Clear();
            IEnumerable<INoteControlChild> renderFinal = PartialInnerNotes;

            foreach (var filter in Filters)
                renderFinal = RecursiveFiltering(renderFinal, filter);

            foreach (var child in renderFinal)
            {
                if (child is Control ctrl)
                    ctrl.Dock = DockStyle.Top;
                if (child is NoteControlParent asParent)
                    SetupAddNoteButton(asParent);
            }

            PanelContainer.Controls.AddRange(
                renderFinal
                    .Reverse()
                    .Where(x => x is Control)
                    .Cast<Control>()
                    .ToArray()
            );
            PanelContainer.ResumeLayout();
        }
    }
}
