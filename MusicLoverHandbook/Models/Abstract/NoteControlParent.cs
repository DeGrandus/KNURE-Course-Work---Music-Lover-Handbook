using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlParent
        : NoteControl,
          INoteParent,
          INoteControlParent
    {
        public ObservableCollection<INoteControlChild> InnerNotes { get; set; } = new();
        public ContentLinker Linker { get; }
        public int Offset { get; set; } = sizeS / 2;
        public Panel InnerContentPanel { get; }
        protected TableLayoutPanel TableOffsetter { get; }

        IReadOnlyCollection<INoteChild> INoteParent.InnerNotes => InnerNotes.Select(x => (INoteChild)x).ToList();

        public bool isOpened { get; set; } = false;

        protected NoteControlParent(string text, string description) : base(text, description)
        {
            TableOffsetter = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0)
            };
            TableOffsetter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Offset));
            TableOffsetter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            TableOffsetter.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(TableOffsetter);
            Controls.SetChildIndex(TableOffsetter, 0);
            InnerContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                Margin = new Padding(0)
            };
            TableOffsetter.Controls.Add(InnerContentPanel, 1, 0);

            Linker = new ContentLinker(this);
            TextLabel.DoubleClick += (sender, e) =>
            {
                if (InnerNotes.Count == 0)
                    return;
                Debug.WriteLine(isOpened);
                isOpened = !isOpened;
                UpdateSize();
            };
        }

        public void AddNote(NoteControl note)
        {
            note.Dock = DockStyle.Top;
            InnerContentPanel.Controls.Add(note);
            InnerContentPanel.Controls.SetChildIndex(note, 0);
            note.SetupColorTheme(note.Type);
            UpdateSize();
        }

        public void RemoveNote(NoteControl note)
        {
            InnerContentPanel.Controls.Remove(note);
            UpdateSize();
        }

        public void MoveNote(NoteControl note, int newIndex)
        {
            InnerContentPanel.Controls.SetChildIndex(note, newIndex);
            UpdateSize();
        }

        public void ReplaceNote(NoteControl oldNote, NoteControl newNote, int newIndex)
        {
            RemoveNote(oldNote);
            AddNote(newNote);
            MoveNote(newNote, newIndex);
            UpdateSize();
        }

        public void ResetNotes()
        {
            InnerContentPanel.Controls.Clear();
            UpdateSize();
        }

        public virtual void UpdateSize()
        {
            var innerHeight = InnerContentPanel.Controls
                .Cast<Control>()
                .Select(c => c.Size.Height)
                .Aggregate((c, n) => c + n);
            var baseHeight = TextLabel.Size.Height;
            Size = isOpened == true ? new(Width, innerHeight + baseHeight) : new(Width, baseHeight);
            Debug.WriteLine(Size);
        }
    }
}
