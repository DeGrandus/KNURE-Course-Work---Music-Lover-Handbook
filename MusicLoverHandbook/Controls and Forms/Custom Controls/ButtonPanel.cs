using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class ButtonPanel : Panel
    {
        public ButtonType ButtonType { get; }

        public int OrderIndex { get; set; }

        public ButtonPanel(ButtonType type, int orderIndex)
        {
            OrderIndex = orderIndex;
            ButtonType = type;
            Margin = Padding = new Padding(0);
        }
    }
}