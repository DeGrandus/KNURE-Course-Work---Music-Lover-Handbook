﻿using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControl : INote, IControlTheme
    {
        Control.ControlCollection Controls { get; }
        Image? Icon { get; set; }
        new string NoteDescription { get; set; }
        new string NoteName { get; set; }
        NoteType NoteType { get; }
        NoteCreationOrder? UsedCreationOrder { get; }

        void ChangeSize(int size);
    }
}