using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models
{
    public class ColorThemeEventArgs : EventArgs
    {
        public Color Color { get; }

        public ColorThemeEventArgs(Color color)
        {
            Color = color;
        }
    }
}
