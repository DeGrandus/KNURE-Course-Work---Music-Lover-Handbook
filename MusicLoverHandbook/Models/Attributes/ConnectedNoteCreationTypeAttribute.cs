using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Attributes
{
    public class ConnectedNoteCreationTypeAttribute : Attribute
    {
        public NoteCreationOrder Type { get; }

        public ConnectedNoteCreationTypeAttribute(NoteCreationOrder type)
        {
            Type = type;
        }
    }
}
