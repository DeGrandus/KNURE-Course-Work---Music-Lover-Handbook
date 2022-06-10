using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicLoverHandbook.Models.Inerfaces.IControlTheme;

namespace MusicLoverHandbook.Models
{
    public abstract class NoteControl : UserControl, INoteControl, IControlTheme
    {
        public abstract NoteType Type { get; }
        public Image? Icon { get; set; }
        public int Offset { get; set; } = 10;
        public int TrueOffset { get; set; }
        public NoteControlOffsetType OffsetType { get; set; } = NoteControlOffsetType.Relative;
        public string NoteText { get; set; }
        public string NoteDescription { get; set; }
        ControlCollection INoteControl.Controls => Controls;

        private Color theme;

        public event ThemeChangeEventHandler ColorChanged;

        public Color ThemeColor { get => theme; set { theme = value; OnColorChanged(); } }
        public void OnColorChanged()
        {
            if (ColorChanged != null)
                ColorChanged(this, new(ThemeColor));
        }
        protected NoteControl(string text, string description)
        {
            NoteText = text;
            NoteDescription = description;
            ThemeColor = Type.GetColor();
        }
    }

}
