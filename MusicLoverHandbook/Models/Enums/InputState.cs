using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models.Enums
{
    public enum InputState : int
    {
        [Text("Data will be put in already created note")]
        OK = 0xCBE4C1,
        [Text("Note with this name is absent. It will be created")]
        CREATION = 0x9CB6E8,
        [Text(@"Field is empty. Interpreted as ""Unknown""")]
        UNKNOWN = 0xFFEDC0,

        //ERRORS
        [Text("This field can't be empty")]
        [ErrorState]
        EMPTY_FIELD = 0xF89198
    }
    public class TextAttribute : Attribute
    {
        public string Text { get; }
        public TextAttribute(string text)
        {
            Text = text;
        }
    }
    public class ErrorStateAttribute : Attribute
    {
    }
    static class ExtensionMethods
    {
        public static string? GetStringValue(this InputState value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            TextAttribute[]? attrs = fieldInfo?.GetCustomAttributes(typeof(TextAttribute), false).Cast<TextAttribute>().ToArray();
            return attrs?.Length > 0 ? attrs[0].Text : null;
        }
        public static bool? IsError(this InputState value)
        {
            return value.GetType().GetField(value.ToString())?.GetCustomAttribute<ErrorStateAttribute>() != null;
        }
    }

}
