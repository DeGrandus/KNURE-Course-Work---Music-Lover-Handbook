using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    internal class SmartComboBox : ComboBox, ISmartComboBox
    {
        public SmartComboBox(InputType inputType)
        {
            InputType = inputType;
        }

        private InputState state;
        public InputState State
        {
            get => state;
            set
            {
                BackColor = Color.FromArgb(255, Color.FromArgb((int)value));
                state = value;
            }
        }
        public InputType InputType
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
