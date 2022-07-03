﻿using MusicLoverHandbook.Models.JSON;
using MusicLoverHandbook.Models.NoteAlter;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControl : INote, INoteSerializable, INoteWidelyComparable
    {
        #region Public Properties

        Control.ControlCollection Controls { get; }
        Image? Icon { get; set; }
        public bool IsDeleteShown { get; set; }
        public bool IsEditShown { get; set; }
        public bool IsInfoShown { get; set; }
        public Color MainColor { get; set; }
        new string NoteDescription { get; set; }
        new string NoteName { get; set; }

        #endregion Public Properties

        #region Public Methods

        void ChangeSize(int size);

        List<LiteNote> Flatten();

        public void InvokeActionHierarcaly(Action<INoteControl> action);

        LiteNote SingleFlatten();

        #endregion Public Methods
    }

    public interface INoteSerializable
    {
        #region Public Methods

        INoteControl Clone();

        NoteImportModel Deserialize();

        string Serialize();

        #endregion Public Methods
    }

    public interface INoteWidelyComparable
    {
        #region Public Methods

        bool Equals(object? obj);

        int GetHashCode();

        bool RoughEquals(object? obj);

        #endregion Public Methods
    }
}
