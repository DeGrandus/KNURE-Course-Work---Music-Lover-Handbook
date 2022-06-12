using MusicLoverHandbook.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class ButtonPanel : Panel
    {
        public int OrderIndex { get; set; }
        public ButtonType ButtonType { get; }
        public ButtonPanel(ButtonType type, int orderIndex)
        {
            OrderIndex = orderIndex;
            ButtonType = type;
            Margin = Padding = new Padding(0);
        }

    }
}
