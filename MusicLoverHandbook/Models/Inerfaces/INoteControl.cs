namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControl : INote, IControlTheme
    {
        Image? Icon { get; set; }
        string NoteDescription { get; set; }
        string NoteText { get; set; }
        Control.ControlCollection Controls { get; }
    }
}
