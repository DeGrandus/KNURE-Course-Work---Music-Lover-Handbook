namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControlChild : INoteControl
    {
        IControlParent ParentNote { get; set; }
    }
}
