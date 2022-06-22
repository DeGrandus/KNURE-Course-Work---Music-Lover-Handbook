﻿using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteDisc : NoteControlMidder
    {
        public NoteDisc(
            IControlParent author,
            string text,
            string description,
            NoteCreationOrder? usedCreationOrder
        ) : base(author, text, description)
        {
            InitializeComponent();
            Icon = Properties.Resources.disc;
            UsedCreationOrder = usedCreationOrder;
        }

        public override NoteType NoteType { get; } = NoteType.Disc;
        public override NoteCreationOrder? UsedCreationOrder { get; }
    }
}