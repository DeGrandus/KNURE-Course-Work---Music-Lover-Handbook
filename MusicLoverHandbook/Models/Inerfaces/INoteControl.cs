using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControl : INote, IControlTheme
    {
        NoteType NoteType { get; }
        Image? Icon { get; set; }
        new string NoteDescription { get; set; }
        new string NoteName { get; set; }
        Control.ControlCollection Controls { get; }
        void ChangeSize(int size);
    }
}
