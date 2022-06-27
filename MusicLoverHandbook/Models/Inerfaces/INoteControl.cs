using MusicLoverHandbook.Models.JSON;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControl : INote, IControlTheme, INoteSerializable, INoteWidelyComparable
    {
        Control.ControlCollection Controls { get; }
        Image? Icon { get; set; }
        public bool IsDeleteShown { get; set; }
        public bool IsEditShown { get; set; }
        public bool IsInfoShown { get; set; }
        new string NoteDescription { get; set; }
        new string NoteName { get; set; }

        void ChangeSize(int size);

        List<NoteLite> Flatten();

        public void InvokeActionHierarcaly(Action<INoteControl> action);

        NoteLite SingleFlatten();
    }

    public interface INoteSerializable
    {
        INoteControl Clone();

        NoteRawImportModel Deserialize();

        string Serialize();
    }

    public interface INoteWidelyComparable
    {
        bool Equals(object? obj);

        int GetHashCode();

        bool RoughEquals(object? obj);
    }
}
