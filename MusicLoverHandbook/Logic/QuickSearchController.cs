using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Delegates;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Inerfaces;
using System.Diagnostics;

namespace MusicLoverHandbook.Logic
{
    public class QuickSearchController
    {
        #region Private Fields

        private NotesContainer container;
        private QuickSearchResultHandler? resultsChanged;
        private IEnumerable<INoteControlChild> searchResults;

        #endregion Private Fields

        #region Public Properties

        public bool IsDescriptionIncluded { get; set; } = false;

        #endregion Public Properties

        #region Private Properties

        private IEnumerable<INoteControlChild> searchInSource => container.CurrentlyActiveNotes;

        #endregion Private Properties

        #region Public Constructors

        public QuickSearchController(NotesContainer container)
        {
            this.container = container;
        }

        #endregion Public Constructors

        #region Public Methods

        public void InvokeQuickSearch(string compareString)
        {
            Debug.WriteLine(compareString);
            Debug.WriteLine(searchInSource.Count());
            if (compareString == "")
            {
                searchResults = searchInSource;
            }
            else
            {
                List<INoteControlChild> results = new();
                foreach (var child in searchInSource)
                    if (child is INoteControlParent asParent)
                        if (
                            CheckForSearchString(asParent, compareString, new())
                            is INoteControlChild validResult
                        )
                            results.Add(validResult);
                searchResults = results;
            }
            OnResultsChanged();
        }

        #endregion Public Methods

        #region Protected Methods

        protected void OnResultsChanged()
        {
            if (resultsChanged != null)
                resultsChanged(searchResults);
        }

        #endregion Protected Methods

        #region Private Methods

        private INoteControlChild? CheckForSearchString(
            INoteControlParent parent,
            string matchString,
            LinkedList<INoteControlParent> insideOf
        )
        {
            if (insideOf.Count == 0)
            {
                if (MakeMatch(parent, matchString))
                    return parent as INoteControlChild;
                insideOf.AddFirst(parent);
            }

            var innerNotes = parent.InnerNotes;
            matchString = matchString.ToLower();

            INoteControlChild? output = null;
            foreach (var note in innerNotes)
            {
                if (output != null)
                    return output;

                if (!note.NoteType.IsInformaionCarrier())
                    continue;

                if (MakeMatch(note, matchString))
                {
                    foreach (var insider in insideOf)
                        if (!insider.IsOpened)
                            insider.SwitchOpenState();
                    if (insideOf.First?.Value is INoteControlChild asChild)
                    {
                        output = asChild;
                    }
                }
                if (note is INoteControlParent asParent)
                {
                    output =
                        CheckForSearchString(
                            asParent,
                            matchString,
                            new(insideOf.Concat(new[] { asParent }))
                        ) ?? output;
                }
            }
            return output;
        }

        private bool MakeMatch(INoteControl note, string matchString)
        {
            var resultNameMatch = note.NoteName.ToLower().Contains(matchString);
            var resultDescriptionMatch = note.NoteDescription.ToLower().Contains(matchString);
            return resultNameMatch || (IsDescriptionIncluded ? resultDescriptionMatch : false);
        }

        #endregion Private Methods

        #region Public Delegates


        #endregion Public Delegates

        #region Public Events

        public event QuickSearchResultHandler ResultsChanged
        {
            add => resultsChanged += value;
            remove => resultsChanged -= value;
        }

        #endregion Public Events
    }
}
