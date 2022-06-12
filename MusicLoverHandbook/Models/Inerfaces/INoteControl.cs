using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControl : INote, IControlTheme
    {
        Image? Icon { get; set; }
        NoteType NoteType { get; }
        string NoteDescription { get; set; }
        string NoteText { get; set; }
        Control.ControlCollection Controls { get; }
        void ChangeSize(int size);
    }
}
