namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteChild : INote
    {
        #region Public Properties

        public new INoteParent ParentNote { get; }

        #endregion Public Properties
    }
}
