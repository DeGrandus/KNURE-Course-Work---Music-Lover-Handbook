using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteAdd : NoteControlChild<NoteControlParent<NoteControl>>
    {
        public override NoteType Type { get; } = NoteType.AddButton;

        public NoteAdd(
            NoteControlParent<NoteControl> parent,
            string noteText,
            string noteDescription
        ) : base(parent, noteText, noteDescription)
        {
            InitializeComponent();
        }
    }
}
