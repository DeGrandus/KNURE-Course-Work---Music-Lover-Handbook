using MusicLoverHandbook.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models.Inerfaces
{
    internal class NoteControl : UserControl, INote
    { 
        Image? Icon { get; set; }
        int Offset { get; set; } = 10;
        NoteControlOffsetType OffsetType { get; set; } = NoteControlOffsetType.Relative;
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
    internal abstract class NoteControl<InnterContentType> : NoteControl, INote<InnterContentType> where InnterContentType : NoteControl
    {
        ObservableCollection<InnterContentType> InnerContent { get; set; } = new();
        IReadOnlyCollection<InnterContentType> INote<InnterContentType>.InnerContent => InnerContent.ToList();

        protected NoteControl(Color stripColor, string text, string description) : base(stripColor, text, description)
        {

        }
    }
}
