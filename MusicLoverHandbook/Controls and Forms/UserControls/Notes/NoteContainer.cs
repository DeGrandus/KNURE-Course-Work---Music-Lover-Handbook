using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MusicLoverHandbook.Models.Inerfaces.IControlTheme;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteContainer : NoteControlParent<NoteAuthor>
    {
        public override NoteType Type { get; } = NoteType.Container;
        public NoteContainer(string text, string description) : base(text, description)
        {
            InitializeComponent();
        }      
    }
}
