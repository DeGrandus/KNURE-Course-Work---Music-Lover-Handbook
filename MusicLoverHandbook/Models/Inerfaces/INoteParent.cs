namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteParent : INote
    {
        #region Public Properties

        public IReadOnlyCollection<INoteChild> InnerNotes { get; }

        #endregion Public Properties
    }
}
