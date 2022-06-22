namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteParent
    {
        public IReadOnlyCollection<INoteChild> InnerNotes { get; }
    }
}