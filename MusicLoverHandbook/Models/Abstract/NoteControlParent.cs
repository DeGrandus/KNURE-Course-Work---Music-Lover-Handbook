using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlParent : NoteControl, INoteParent, INoteControlParent
    {
        #region Private Fields

        private ObservableCollection<INoteControlChild> innerNotes = new();

        #endregion Private Fields

        #region Public Properties

        public Panel InnerContentPanel { get; }

        public ObservableCollection<INoteControlChild> InnerNotes
        {
            get => innerNotes;
            set
            {
                innerNotes = value;
                Debug.WriteLine("Setting new Inner Notes: ");
                Debug.WriteLine(String.Join("\n", value.Select(x => x.NoteName)));
                Debug.WriteLine("Setting new Inner Notes END");

                SetupLinker();
            }
        }

        IReadOnlyCollection<INoteChild> INoteParent.InnerNotes =>
            InnerNotes.Select(x => (INoteChild)x).ToList();

        public bool IsOpened { get; set; } = false;

        public ContentLinker Linker { get; private set; }

        public int Offset { get; set; }

        #endregion Public Properties



        #region Protected Properties

        protected TableLayoutPanel TableOffsetter { get; }

        #endregion Protected Properties



        #region Protected Constructors

        protected NoteControlParent(
            string text,
            string description,
            NoteType noteType,
            NoteCreationOrder? order
        ) : base(text, description, noteType, order)
        {
            Offset = sizeS * 2 / 3;
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
        }

        #endregion Protected Constructors



        #region Public Methods

        public void AddNote(NoteControl note, ContentLinker linker)
        {
            note.Dock = DockStyle.Top;
            if (note is INoteControlChild child)
                child.ParentNote = this;
            InnerContentPanel.Controls.Add(note);
            InnerContentPanel.Controls.SetChildIndex(note, 0);
            note.SetupColorTheme(note.NoteType);
            UpdateSize();
        }

        public void AddNotes(NoteControl[] notes, ContentLinker linker)
        {
            foreach (var note in notes)
            {
                note.Dock = DockStyle.Top;
                note.SetupColorTheme(note.NoteType);
                if (note is INoteControlChild child)
                    child.ParentNote = this;
            }
            InnerContentPanel.Controls.AddRange(notes.ToArray());
            UpdateSize();
        }

        public void ChangeSizeHierarchically(int size)
        {
            ChangeSize(size);
            foreach (var note in InnerNotes)
                if (note is NoteControlMidder midder)
                    midder.ChangeSizeHierarchically(size);
                else
                    note.ChangeSize(size);
        }

        public override List<LiteNote> Flatten()
        {
            var curr = base.Flatten();
            foreach (var note in InnerNotes)
                if (note.NoteType.IsInformaionCarrier())
                    curr = curr.Concat(note.Flatten()).ToList();
            return curr;
        }

        public override void InvokeActionHierarcaly(Action<INoteControl> action)
        {
            base.InvokeActionHierarcaly(action);
            foreach (var note in InnerNotes)
                note.InvokeActionHierarcaly(action);
        }

        public void MoveNote(NoteControl note, int newIndex, ContentLinker linker)
        {
            InnerContentPanel.Controls.SetChildIndex(note, newIndex);
            UpdateSize();
        }

        public void OnDoubleClick()
        {
            SwitchOpenState();
        }

        public void RemoveNote(NoteControl note, ContentLinker linker)
        {
            InnerContentPanel.Controls.Remove(note);
            if (note is INoteControlChild child)
                child.ParentNote = null;
            UpdateSize();
        }

        public void ReplaceNote(
            NoteControl oldNote,
            NoteControl newNote,
            int newIndex,
            ContentLinker linker
        )
        {
            RemoveNote(oldNote, linker);
            AddNote(newNote, linker);
            MoveNote(newNote, newIndex, linker);
            UpdateSize();
        }

        public void ResetNotes(ContentLinker linker)
        {
            InnerContentPanel.Controls.Clear();
            UpdateSize();
        }

        public void SwitchOpenState()
        {
            if (InnerNotes.Count == 0)
                return;
            IsOpened = !IsOpened;
            UpdateSize();
        }

        public override string ToString()
        {
            return base.ToString()
                + "\n"
                + string.Join('\n', InnerNotes.Select(x => "- " + x.ToString()));
        }

        public virtual void UpdateSize()
        {
            SuspendLayout();
            var innerHeight = InnerContentPanel.Controls
                .Cast<Control>()
                .Select(c => c.Size.Height)
                .Concat(new[] { 0 })
                .Aggregate((c, n) => c + n);
            var baseHeight = TextLabel.Size.Height;
            Size = IsOpened == true ? new(Width, innerHeight + baseHeight) : new(Width, baseHeight);
            //Debug.WriteLine(Size);
            ResumeLayout();
        }

        #endregion Public Methods



        #region Protected Methods

        protected override void SetupLayout()
        {
            base.SetupLayout();
            TextLabel.DoubleClick += (sender, e) => OnDoubleClick();
        }

        #endregion Protected Methods

        #region Private Methods

        private void SetupLinker()
        {
            Linker = new ContentLinker(this);
        }

        #endregion Private Methods
    }
}
