using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models
{
    public class NoteControl : UserControl, INote
    {
        public Image? Icon { get; set; }
        public int Offset { get; set; } = 10;
        public int TrueOffset { get; set; }
        public NoteControlOffsetType OffsetType { get; set; } = NoteControlOffsetType.Relative;
        public Color StripColor { get; set; }
        public string NoteText { get; set; }
        public string NoteDescription { get; set; }
        protected NoteControl(Color stripColor, string text, string description)
        {
            StripColor = stripColor;
            NoteText = text;
            NoteDescription = description;
        }
    }

}
