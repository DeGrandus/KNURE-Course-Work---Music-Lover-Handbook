using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Attributes
{
    public class AssociatedNoteCreationTypeAttribute : Attribute
    {
        public NoteCreationOrder Type { get; }

        public AssociatedNoteCreationTypeAttribute(NoteCreationOrder type)
        {
            Type = type;
        }
    }
}
