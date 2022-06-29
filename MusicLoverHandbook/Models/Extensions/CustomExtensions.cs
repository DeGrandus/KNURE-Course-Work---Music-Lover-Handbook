using MusicLoverHandbook.Models.Attributes;
using MusicLoverHandbook.Models.Enums;
using System.Reflection;

namespace MusicLoverHandbook.Models.Extensions
{
    public static class CustomExtensions
    {
        public static Color? GetColor(this NoteType value)
        {
            return value
                .GetType()
                .GetField(value.ToString())
                ?.GetCustomAttribute<EnumColorAttribute>(false)
                ?.ColorMain;
        }

        public static Color? GetLiteColor(this NoteType value)
        {
            return value
                .GetType()
                .GetField(value.ToString())
                ?.GetCustomAttribute<EnumColorAttribute>(false)
                ?.ColorLite;
        }

        public static bool IsInformaionCarrier(this NoteType value)
        {
            return value
                    .GetType()
                    .GetField(value.ToString())!
                    .GetCustomAttribute<InformationCarrierAttribute>() != null;
        }
        public static NoteCreationOrder? GetConnectedCreationOrder(this NoteType type)
        {
            return type.GetType()
                .GetField(type.ToString())
                ?.GetCustomAttribute<ConnectedNoteCreationTypeAttribute>()
                ?.Type;
        }

        public static Type? GetConnectedNoteType(this NoteType type)
        {
            return type.GetType()
                .GetField(type.ToString())
                ?.GetCustomAttribute<ConnectedNoteTypeAttribute>()
                ?.ConnectedType;
        }

        public static string ToString(this Enum type, bool useCustomStringValue)
        {
            if (useCustomStringValue)
                return type.GetType()
                        .GetField(type.ToString())
                        ?.GetCustomAttribute<StringValueAttribute>()
                        ?.Value ?? type.ToString();
            return type.ToString();
        }
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
        public static string? GetStringValue(this InputStatus value)
        {
            return
                value.GetType().GetField(value.ToString())?.GetCustomAttribute<TextAttribute>(false)
                    is TextAttribute attr
              ? attr.Text
              : null;
        }

        public static bool IsError(this InputStatus value)
        {
            return value
                    .GetType()
                    .GetField(value.ToString())
                    ?.GetCustomAttribute<ErrorStateAttribute>(false) != null;
        }
    }
}
