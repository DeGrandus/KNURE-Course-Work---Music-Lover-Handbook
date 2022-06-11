using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using static System.Windows.Forms.Control;

namespace MusicLoverHandbook.Models
{
    public class ContentLinker<InnerNotesType> where InnerNotesType : INoteControl
    {
        public NoteControlParent<InnerNotesType> Note { get; }
        public ObservableCollection<InnerNotesType> Observed => Note.InnerNotes;

        public ContentLinker(NoteControlParent<InnerNotesType> note)
        {
            Note = note;
            Observed.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                        foreach (NoteControl item in e.NewItems)
                            Note.AddNote(item);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                        foreach (NoteControl item in e.OldItems)
                            Note.RemoveNote(item);
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
                            Note.ReplaceNote(item.First, item.Second, ind);
                        }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.NewItems != null && e.NewItems.Count == 1 && e.NewItems[0] is NoteControl)
                        Note.MoveNote((e.NewItems[0] as NoteControl), e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (sender is ObservableCollection<NoteControl> && e.OldItems != null)
                        Note.ResetNotes();
                    break;
            }
        }
    }
}
