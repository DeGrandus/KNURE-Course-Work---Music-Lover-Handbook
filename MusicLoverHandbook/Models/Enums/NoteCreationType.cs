using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum NoteCreationOrder
    {
        [Order(NoteType.Author, NoteType.Disc, NoteType.Song, NoteType.SongFile)]
        AuthorThenDisc,

        [Order(NoteType.Disc, NoteType.Author, NoteType.Song, NoteType.SongFile)]
        DiscThenAuthor
    }

    public static class NoteCreationOrderExtensions
    {
        public static LinkedList<NoteType> GetOrder(this NoteCreationOrder value)
        {
            var orderAttr = value
                .GetType()
                .GetField(value.ToString())!
                .GetCustomAttribute<OrderAttribute>();
            if (orderAttr == null)
                throw new MissingRequiredAttributeException(value, typeof(OrderAttribute));
            return orderAttr.Order;
        }
    }

    public class MissingRequiredAttributeException : Exception
    {
        public MissingRequiredAttributeException(object source, Type missingAttirbute)
            : base(Decorate(source, missingAttirbute))
        {
            Source = source.ToString();
        }

        public override string Message => base.Message;

        private static string Decorate(object source, Type missing)
        {
            return $"{source.GetType()} \"{source}\" missing attribute: {missing}";
        }
    }

    public class OrderAttribute : Attribute
    {
        public LinkedList<NoteType> Order;

        public OrderAttribute(params NoteType[] types)
        {
            Order = new LinkedList<NoteType>(types);
        }
    }
}
