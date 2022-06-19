﻿using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControlChild : NoteControl, INoteChild, INoteControlChild
    {
        private bool inited = false;
        public IControlParent ParentNote { get; set; }

        INoteParent INoteChild.ParentNote => (INoteParent)ParentNote;

        private delegate void DelayedSetup();
        private DelayedSetup? delayedSetup;

        protected NoteControlChild(IControlParent parent, string text, string description)
            : base(text, description)
        {
            ParentNote = parent;
            if (delayedSetup != null)
                delayedSetup();
            inited = true;
        }

        public override void SetupColorTheme(NoteType type)
        {
            var themeColor = () => type.GetColor() ?? Color.Transparent;
            if (inited)
                ThemeColor = themeColor();
            else
                delayedSetup += () =>
                {
                    ThemeColor = themeColor();
                };
        }

        protected override void InitLayout()
        {
            if (inited)
                base.InitLayout();
            else
                delayedSetup += base.InitLayout;
        }

        protected override void InitValues(string text, string description)
        {
            if (inited)
                base.InitValues(text, description);
            else
                delayedSetup += () => base.InitValues(text, description);
        }
    }
}