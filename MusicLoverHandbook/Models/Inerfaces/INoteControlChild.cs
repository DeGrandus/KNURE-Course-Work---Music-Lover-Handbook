namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControlChild : INoteControl
    {
        INoteControlParent ParentNote { get; set; }
    }
}
