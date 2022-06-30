namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteControlChild : INoteControl, INoteChild
    {
        #region Public Properties

        new IParentControl ParentNote { get; set; }

        #endregion Public Properties

        #region Public Methods

        bool ContainsInParentTree(IContainerControl potentialParent);

        INoteControlParent? GetFirstNoteControlParent();

        IParentControl GetFirstParent();

        #endregion Public Methods
    }
}
