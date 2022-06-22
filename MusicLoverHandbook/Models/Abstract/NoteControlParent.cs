﻿using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlParent
        : NoteControl,
          INoteParent,
          INoteControlParent,
          IControlParent
    {
        protected NoteControlParent(string text, string description) : base(text, description)
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

        public Panel InnerContentPanel { get; }
        public ObservableCollection<INoteControlChild> InnerNotes { get; set; } = new();

        IReadOnlyCollection<INoteChild> INoteParent.InnerNotes =>
            InnerNotes.Select(x => (INoteChild)x).ToList();

        public bool IsOpened { get; set; } = false;
        public ContentLinker Linker { get; }
        public int Offset { get; set; }
        protected TableLayoutPanel TableOffsetter { get; }

        public void AddNote(NoteControl note)
        {
            note.Dock = DockStyle.Top;
            InnerContentPanel.Controls.Add(note);
            InnerContentPanel.Controls.SetChildIndex(note, 0);
            note.SetupColorTheme(note.NoteType);
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

        public void MoveNote(NoteControl note, int newIndex)
        {
            InnerContentPanel.Controls.SetChildIndex(note, newIndex);
            UpdateSize();
        }

        public void OnDoubleClick()
        {
            SwitchOpenState();
        }

        public void RemoveNote(NoteControl note)
        {
            InnerContentPanel.Controls.Remove(note);
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

        public void SwitchOpenState()
        {
            if (InnerNotes.Count == 0)
                return;
            IsOpened = !IsOpened;
            UpdateSize();
        }

        public override string ToString()
        {
            return $"{NoteName}: [ {string.Join(", ", InnerNotes)} ]";
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
            Debug.WriteLine(Size);
            ResumeLayout();
        }

        protected override void InitCustomLayout()
        {
            base.InitCustomLayout();
            TextLabel.DoubleClick += (sender, e) => OnDoubleClick();
        }
    }
}