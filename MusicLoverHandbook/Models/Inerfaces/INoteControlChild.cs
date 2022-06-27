namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControlChild : INoteControl
    {
        IParentControl ParentNote { get; set; }

        bool ContainsInParentTree(IContainerControl potentialParent);

        INoteControlParent? GetFirstNoteControlParent();

        IParentControl GetFirstParent();
    }
}
