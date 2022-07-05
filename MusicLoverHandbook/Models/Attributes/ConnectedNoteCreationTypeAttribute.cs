using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Attributes
{
    public class AssociatedNoteCreationTypeAttribute : Attribute
    {
        #region Public Properties

        public NoteCreationOrder Type { get; }

        #endregion Public Properties

        #region Public Constructors + Destructors

        public AssociatedNoteCreationTypeAttribute(NoteCreationOrder type)
        {
            Type = type;
        }

        #endregion Public Constructors + Destructors
    }
}
