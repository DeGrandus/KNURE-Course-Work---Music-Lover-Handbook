﻿using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteDisc : NoteControlMidder
    {
        public override NoteType NoteType { get; } = NoteType.Disc;

        public NoteDisc(IControlParent author, string text, string description)
            : base(author, text, description)
        {
            InitializeComponent();
            Icon = Properties.Resources.disc;
        }
    }
}
