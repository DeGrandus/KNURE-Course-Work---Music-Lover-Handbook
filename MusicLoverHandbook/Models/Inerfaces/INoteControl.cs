using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.JSON;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteSerializable
    {
        string Serialize();
        NoteRawImportModel Deserialize();
        INoteControl Clone();
    }
    public interface INoteWidelyComparable
    {
        bool RoughEquals(object? obj);
        bool Equals(object? obj);
        int GetHashCode();
    }
    public interface INoteControl : INote, IControlTheme, INoteSerializable, INoteWidelyComparable
    {
        Control.ControlCollection Controls { get; }
        Image? Icon { get; set; }
        new string NoteDescription { get; set; }
        new string NoteName { get; set; }
        void ChangeSize(int size);

        NoteLite SingleFlatten();
        List<NoteLite> Flatten();
    }
}
