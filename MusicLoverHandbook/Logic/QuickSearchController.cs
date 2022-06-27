using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Diagnostics;

namespace MusicLoverHandbook.Logic
{
    public class QuickSearchController
    {
        private IEnumerable<INoteControlChild> searchResults;
        private IEnumerable<INoteControlChild> searchingSource => container.CurrentlyActiveNotes;
        private QuickSearchResultEventHandler? resultsChanged;
        private NotesContainer container;
        public QuickSearchController(
            NotesContainer container
        )
        {
            this.container = container;
        }

        public delegate void QuickSearchResultEventHandler(IEnumerable<INoteControlChild> QSResult);

        public event QuickSearchResultEventHandler ResultsChanged
        {
            add => resultsChanged += value;
            remove => resultsChanged -= value;
        }

        protected void OnResultsChanged()
        {
            if (resultsChanged != null)
                resultsChanged(searchResults);
        }

        private INoteControlChild? CheckForSearchString(
            INoteControlParent parent,
            string value,
            LinkedList<INoteControlParent> insideOf
        )
        {
            if (insideOf.Count == 0)
            {
                if (MakeMatch(parent, value))
                    return parent as INoteControlChild;
                insideOf.AddFirst(parent);
            }

            var insides = parent.InnerNotes;

            value = value.ToLower();

            INoteControlChild? output = null;
            foreach (var note in insides)
            {
                if (output != null)
                    return output;

                if (!note.NoteType.IsInformaionCarrier())
                    continue;

                if (MakeMatch(note, value))
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
                            value,
                            new(insideOf.Concat(new[] { asParent }))
                        ) ?? output;
                }
            }
            return output;
        }

        private bool MakeMatch(INoteControl note, string toMatch)
        {
            var resultNameMatch = note.NoteName.ToLower().Contains(toMatch);
            var resultDescriptionMatch = note.NoteDescription.ToLower().Contains(toMatch);
            return resultNameMatch || (IsDescriptionIncluded ? resultDescriptionMatch : false);
        }
        public bool IsDescriptionIncluded { get; set; } = false;
        public void InvokeQuickSearch(string compareString)
        {
            Debug.WriteLine(compareString);
            Debug.WriteLine(searchingSource.Count());
            if (compareString == "")
            {

                searchResults = searchingSource;
            }
            else
            {
                List<INoteControlChild> results = new();
                foreach (var child in searchingSource)
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
    }
}
