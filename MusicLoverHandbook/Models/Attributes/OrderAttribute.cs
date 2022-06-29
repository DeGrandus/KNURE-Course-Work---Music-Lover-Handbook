using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Models.Attributes
{
    public class OrderAttribute : Attribute
    {
        public LinkedList<NoteType> Order;

        public OrderAttribute(params NoteType[] types)
        {
            Order = new LinkedList<NoteType>(types);
        }
    }
}
