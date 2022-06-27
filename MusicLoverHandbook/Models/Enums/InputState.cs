using MusicLoverHandbook.Models.Attributes;
using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum InputStatus : int
    {
        NONE = 0xFFFFFF,

        [Text("Data will be put in already created note")]
        OK = 0xCBE4C1,

        [Text("Note with this name is absent. It will be created")]
        CREATION = 0x9CB6E8,

        [Text(@"Field is empty. Interpreted as ""Unknown""")]
        UNKNOWN = 0xFFEDC0,

        [Text("Inactive until above isn't choosen")]
        INACTIVE = 0xFFFFFF,

        [Text("It seems that this name is very similar to some already created note")]
        ANALOG = 0xC7B1D5,

        //ERRORS
        [Text("This field can't be empty")]
        [ErrorState]
        EMPTY_FIELD = 0xF89198,

        [Text("This field is too short")]
        [ErrorState]
        TOO_SHORT
    }

    public static class ExtensionMethods
    {
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