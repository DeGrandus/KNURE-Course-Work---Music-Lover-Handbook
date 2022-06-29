using MusicLoverHandbook.Models.Attributes;
using MusicLoverHandbook.Models.Enums;
using System.Reflection;

namespace MusicLoverHandbook.Models.Extensions
{
    public static class CustomExtensions
    {
        #region Public Methods

        public static NoteCreationOrder? GetAssociatedCreationOrder(this NoteType noteType)
        {
            return noteType
                .GetType()
                .GetField(noteType.ToString())
                ?.GetCustomAttribute<AssociatedNoteCreationTypeAttribute>(false)
                ?.Type;
        }

        public static Color? GetColor(this NoteType noteType)
        {
            return noteType
                .GetType()
                .GetField(noteType.ToString())
                ?.GetCustomAttribute<EnumColorAttribute>(false)
                ?.ColorMain;
        }

        public static Type? GetConnectedNoteType(this NoteType noteType)
        {
            return noteType
                .GetType()
                .GetField(noteType.ToString())
                ?.GetCustomAttribute<AssociatedTypeAttribute>(false)
                ?.Type;
        }

        public static Color? GetLiteColor(this NoteType noteType)
        {
            return noteType
                .GetType()
                .GetField(noteType.ToString())
                ?.GetCustomAttribute<EnumColorAttribute>(false)
                ?.ColorLite;
        }

        public static LinkedList<NoteType> GetOrder(this NoteCreationOrder creationOrder)
        {
            var orderAttr = creationOrder
                .GetType()
                .GetField(creationOrder.ToString())!
                .GetCustomAttribute<OrderAttribute>(false);
            if (orderAttr == null)
                throw new MissingRequiredAttributeException(creationOrder, typeof(OrderAttribute));
            return orderAttr.Order;
        }

        public static string? GetStringValue(this InputStatus inputStatus)
        {
            return
                inputStatus
                    .GetType()
                    .GetField(inputStatus.ToString())
                    ?.GetCustomAttribute<TextAttribute>(false)
                    is TextAttribute attr
              ? attr.Text
              : null;
        }

        public static bool IsError(this InputStatus inputStatus)
        {
            return inputStatus
                    .GetType()
                    .GetField(inputStatus.ToString())
                    ?.GetCustomAttribute<ErrorStateAttribute>(false) != null;
        }

        public static bool IsInformaionCarrier(this NoteType noteType)
        {
            return noteType
                    .GetType()
                    .GetField(noteType.ToString())!
                    .GetCustomAttribute<InformationCarrierAttribute>(false) != null;
        }

        public static string ToString(this Enum enumType, bool useCustomStringValue)
        {
            if (useCustomStringValue)
                return enumType
                        .GetType()
                        .GetField(enumType.ToString())
                        ?.GetCustomAttribute<StringValueAttribute>(false)
                        ?.Value ?? enumType.ToString();
            return enumType.ToString();
        }

        #endregion Public Methods
    }
}