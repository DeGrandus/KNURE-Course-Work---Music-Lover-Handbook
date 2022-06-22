﻿using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace MusicLoverHandbook.Logic
{
    public class QuickSearchController
    {
        private TextBox searchBar;
        private NotesContainer notesContainer;
        public List<INoteControlChild>? SearchResults;
        private BasicSwitchLabel switchLabel;
        public QuickSearchController(TextBox searchBar, NotesContainer container, BasicSwitchLabel switchLabel)
        {
            this.searchBar = searchBar;
            this.switchLabel = switchLabel;
            switchLabel.SpecialStateChanged += (state) => PerformSearching();
            notesContainer = container;
            SearchResults = container.InnerNotes.ToList();
            searchBar.TextChanged += SearchBarTextChanged;

        }
        private void SearchBarTextChanged(object? sender, EventArgs e)
        {
            PerformSearching();
        }
        private void PerformSearching()
        {
            if (searchBar.Text == "")
                SearchResults = null;
            else
            {
                List<INoteControlChild> results = new();
                foreach (var child in notesContainer.InnerNotes)
                    if (child is INoteControlParent asParent)
                        if (CheckForSearchString(asParent, searchBar.Text, new()) is INoteControlChild validResult)
                            results.Add(validResult);
                SearchResults = results;
            }
            OnResultsChanged();
        }
        private bool MakeMatch(INoteControl note, string toMatch)
        {
            var resultNameMatch = note.NoteName.ToLower().Contains(toMatch);
            var resultDescriptionMatch = note.NoteDescription.ToLower().Contains(toMatch);
            return resultNameMatch || (switchLabel.SpecialState ? resultDescriptionMatch : false);
        }
        private INoteControlChild? CheckForSearchString(INoteControlParent parent, string value, LinkedList<INoteControlParent> insideOf)
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

                if (note.NoteType.AsInputType() == null) continue;

                if (MakeMatch(note, value))
                {
                    foreach (var insider in insideOf)
                        if (!insider.IsOpened) insider.SwitchOpenState();
                    if (insideOf.First?.Value is INoteControlChild asChild)
                    {
                        output = asChild;
                    }
                }
                if (note is INoteControlParent asParent)
                {
                    output = CheckForSearchString(asParent, value, new(insideOf.Concat(new[] { asParent }))) ?? output;

                }
            }
            return output;
        }
        public delegate void QuickSearchResultEventHandler(List<INoteControlChild>? QSResult);
        private QuickSearchResultEventHandler? resultsChanged;
        public event QuickSearchResultEventHandler ResultsChanged
        {
            add => resultsChanged += value;
            remove => resultsChanged -= value;
        }
        protected void OnResultsChanged()
        {
            if (resultsChanged != null)
                resultsChanged(SearchResults);
        }
    }
}