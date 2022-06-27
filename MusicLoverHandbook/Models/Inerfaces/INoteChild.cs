namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteChild : INote
    {
        public INoteParent ParentNote { get; }
    }
}