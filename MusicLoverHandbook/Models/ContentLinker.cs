using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MusicLoverHandbook.Models
{
    public class ContentLinker : IParentControl
    {
        #region Public Properties

        public ObservableCollection<INoteControlChild> InnerNotes => LinkingParent.InnerNotes;

        public NoteControlParent LinkingParent { get; }

        #endregion Public Properties



        #region Public Constructors

        public ContentLinker(NoteControlParent linkingParent)
        {
            LinkingParent = linkingParent;

            var linkContent = InnerNotes.ToList();
            InnerNotes.Clear();

            InnerNotes.CollectionChanged += OnCollectionChanged;

            foreach (var cont in linkContent)
                InnerNotes.Add(cont);
        }

        #endregion Public Constructors



        #region Private Methods

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                        foreach (NoteControl item in e.NewItems)
                            LinkingParent.AddNote(item, this);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                        foreach (NoteControl item in e.OldItems)
                            LinkingParent.RemoveNote(item, this);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems != null && e.NewItems != null)
                        foreach (
                            var item in Enumerable.Zip(
                                (List<NoteControl>)e.OldItems,
                                (List<NoteControl>)e.NewItems
                            )
                        )
                        {
                            var ind = LinkingParent.Controls.IndexOf(item.First);
                            LinkingParent.ReplaceNote(item.First, item.Second, ind, this);
                        }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (
                        e.NewItems != null
                        && e.NewItems.Count == 1
                        && e.NewItems[0] is NoteControl note
                    )
                        LinkingParent.MoveNote(note, e.NewStartingIndex, this);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    LinkingParent.ResetNotes(this);
                    break;
            }
        }

        #endregion Private Methods
    }
}
