using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Windows.Forms;

namespace MusicLoverHandbook.Models
{
    public interface INoteControl : INote
    {
        Image? Icon { get; set; }
        string NoteDescription { get; set; }
        string NoteText { get; set; }
        Control.ControlCollection Controls { get; }
    }
}