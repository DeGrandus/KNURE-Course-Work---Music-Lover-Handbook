using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface IControlTheme
    {
        Color ThemeColor { get; set; }
        public event ThemeChangeEventHandler ColorChanged;
        public delegate void ThemeChangeEventHandler(object? sender, ColorThemeEventArgs e);
    }
}
