using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MusicLoverHandbook.Models
{
    public class ContentLinker : IParentControl
    {
        public ObservableCollection<INoteControlChild> InnerNotes => Note.InnerNotes;

        public NoteControlParent Note { get; }

        public ContentLinker(NoteControlParent note)
        {
            Note = note;

            var linkContent = InnerNotes.ToList();
            InnerNotes.Clear();

            InnerNotes.CollectionChanged += OnCollectionChanged;

            foreach (var cont in linkContent)
                InnerNotes.Add(cont);
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                        foreach (NoteControl item in e.NewItems)
                            Note.AddNote(item, this);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                        foreach (NoteControl item in e.OldItems)
                            Note.RemoveNote(item, this);
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
                            var ind = Note.Controls.IndexOf(item.First);
                            Note.ReplaceNote(item.First, item.Second, ind, this);
                        }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (
                        e.NewItems != null
                        && e.NewItems.Count == 1
                        && e.NewItems[0] is NoteControl note
                    )
                        Note.MoveNote(note, e.NewStartingIndex, this);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    Note.ResetNotes(this);
                    break;
            }
        }
    }
}
