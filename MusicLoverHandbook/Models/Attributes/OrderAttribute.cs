using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Attributes
{
    public class OrderAttribute : Attribute
    {
        #region Public Fields

        public LinkedList<NoteType> Order;

        #endregion Public Fields

        #region Public Constructors

        public OrderAttribute(params NoteType[] types)
        {
            Order = new LinkedList<NoteType>(types);
        }

        #endregion Public Constructors
    }
}