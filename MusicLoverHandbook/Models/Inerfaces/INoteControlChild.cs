using MusicLoverHandbook.Models.Abstract;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControlChild : INoteControl
    {
        IParentControl ParentNote { get; set; }
        IParentControl GetFirstParent();
        INoteControlParent? GetFirstNoteControlParent();
        bool ContainsInParentTree(IContainerControl potentialParent);
    }
}
