﻿using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlMidder : NoteControlParent, INoteControlChild, INoteChild
    {
        protected NoteControlMidder(IControlParent parent, string text, string description, NoteType noteType, NoteCreationOrder? order) : base(text, description, noteType, order)
        {
            ParentNote = parent;
        }

        public IControlParent ParentNote { get; set; }
        INoteParent INoteChild.ParentNote => (INoteParent)ParentNote;

        public override void SetupColorTheme(NoteType type)
        {
            ThemeColor =
                type.GetColor()
                ?? (
                    ParentNote is IControlParent asParent
                        ? asParent.InnerNotes.LastOrDefault()?.ThemeColor
                        : null
                )
                ?? Color.Transparent;
        }
        protected override List<(Type Type, object? Data)> ConstructorRequested
        {
            get
            {
                var req = base.ConstructorRequested;
                req.Insert(0, (typeof(IControlParent), ParentNote));
                return req;
            }
        }
        public override void UpdateSize()
        {
            base.UpdateSize();
            if (ParentNote is INoteControlParent noteParent)
                noteParent.UpdateSize();
        }
    }
}