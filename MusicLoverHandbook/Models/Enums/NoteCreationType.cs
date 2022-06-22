using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum NoteCreationOrder
    {
        [Order(InputType.Author, InputType.Disc, InputType.SongName, InputType.SongFile)]
        AuthorThenDisc,

        [Order(InputType.Disc, InputType.Author, InputType.SongName, InputType.SongFile)]
        DiscThenAuthor
    }

    public static class NoteCreationOrderExtensions
    {
        public static LinkedList<InputType> GetOrder(this NoteCreationOrder value)
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
        public LinkedList<InputType> Order;

        public OrderAttribute(params InputType[] types)
        {
            Order = new LinkedList<InputType>(types);
        }
    }
}