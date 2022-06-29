using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public class NotesContainer : IParentControl
    {
        #region Private Fields

        private List<INoteControlChild>? advancedFilteredNotes;

        private List<Func<IEnumerable<INoteControlChild>, IEnumerable<INoteControlChild>>> filters =
            new();

        private List<INoteControlChild> partialInnerNotes = new();
        private TextBox qSTextBox;
        private int refreshDelay = 0;

        private Timer renderRefreshTimer;

        #endregion Private Fields

        #region Public Properties

        public List<INoteControlChild>? AdvancedFilteredNotes
        {
            get => advancedFilteredNotes;
            set
            {
                advancedFilteredNotes = value;
                InvokeQuickSearch();
            }
        }

        public List<INoteControlChild> CurrentlyActiveNotes =>
            AdvancedFilteredNotes ?? InnerNotes.ToList();

        public List<Func<IEnumerable<INoteControlChild>, IEnumerable<INoteControlChild>>> Filters
        {
            get => filters;
            set
            {
                filters = value;
                InvokeQuickSearch();
            }
        }

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

        #endregion Public Properties

        #region Public Constructors

        public NotesContainer(
            Panel panelContainer,
            TextBox QSTextBox,
            BasicSwitchLabel QSSwitchLabel
        )
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
            QSTextBox.TextChanged += (sender, e) => InvokeQuickSearch();

            QSSwitchLabel.SpecialStateChanged += (sender, state) =>
            {
                QSController.IsDescriptionIncluded = state;
                InvokeQuickSearch();
            };
            InnerNotes.CollectionChanged += (sender, e) =>
            {
                qSTextBox.Text = "";
                InvokeQuickSearch();
            };

            renderRefreshTimer = new Timer() { Interval = 1, Enabled = true, };
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

        #endregion Public Constructors

        #region Public Methods

        public void Insert_AddNoteButton(NoteControlParent note)
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
                    Insert_AddNoteButton(innertParent);
            PanelContainer.ResumeLayout();
        }

        public void InvokeQuickSearch() => QSController.InvokeQuickSearch(qSTextBox.Text);

        #endregion Public Methods

        #region Private Methods

        private void ContainmentRefreshing()
        {
            PanelContainer.SuspendLayout();

            PanelContainer.Controls.Clear();
            IEnumerable<INoteControlChild> renderFinal = PartialInnerNotes;

            //Debug.WriteLine(filters.Count);
            renderFinal
                .ToList()
                .ForEach(
                    x =>
                    {
                        if (x is INoteControlParent s)
                            RemoveInformationlessNotes(s);
                    }
                );

            foreach (var filter in Filters)
            {
                //Debug.WriteLine("________________________________________");
                //Debug.WriteLine(String.Join("\n", renderFinal));
                renderFinal = RecursiveFiltering(renderFinal, filter);
                //Debug.WriteLine(String.Join("\n", renderFinal));
            }

            foreach (var child in renderFinal)
            {
                if (child is Control ctrl)
                    ctrl.Dock = DockStyle.Top;
                if (child is NoteControlParent asParent)
                    Insert_AddNoteButton(asParent);
                child.ParentNote = this;
            }

            PanelContainer.Controls.AddRange(
                renderFinal.Reverse().Where(x => x is Control).Cast<Control>().ToArray()
            );
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

        private void DelayedContainmentRefreshing()
        {
            refreshDelay = 20;
        }

        private IEnumerable<INoteControlChild> RecursiveFiltering(
            IEnumerable<INoteControlChild> notes,
            Func<IEnumerable<INoteControlChild>, IEnumerable<INoteControlChild>> filteringFunc
        )
        {
            var output = filteringFunc(notes);
            //Debug.WriteLine(String.Join("\n", notes.Select(x => x.NoteName)));
            //Debug.WriteLine(String.Join("\n", output.Select(x => x.NoteName)));
            foreach (var note in notes)
                if (note is INoteControlParent asParent)
                {
                    //Debug.WriteLine("INSIDE RECUR");
                    //Debug.WriteLine(String.Join("\n", asParent.InnerNotes.Select(x => x.NoteName)));
                    //Debug.WriteLine("---------------------");
                    var k = RecursiveFiltering(asParent.InnerNotes, filteringFunc);
                    //Debug.WriteLine($" SEMI-END INSIDE RECUR {note.NoteType} ");

                    //Debug.WriteLine(String.Join("\n", k));
                    //Debug.WriteLine("INSIDE RECUR END");
                    asParent.InnerNotes = new(k);
                }
            return output;
        }

        private void RemoveInformationlessNotes(INoteControlParent note)
        {
            foreach (var nt in note.InnerNotes.ToList())
            {
                if (!nt.NoteType.IsInformaionCarrier())
                {
                    note.InnerNotes.Remove(nt);
                    continue;
                }
                if (nt is INoteControlParent asP)
                    RemoveInformationlessNotes(asP);
            }
        }

        #endregion Private Methods
    }
}