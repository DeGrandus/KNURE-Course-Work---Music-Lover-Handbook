using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using System.Diagnostics;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteSongFile : NoteControlChild
    {
        public override NoteType NoteType { get; } = NoteType.SongFile;
        public override NoteCreationOrder? UsedCreationOrder { get; }

        public NoteSongFile(
            NoteSong song,
            string text,
            string description,
            NoteCreationOrder? usedCreationOrder
        ) : base(song, text, description)
        {
            InitializeComponent();

            Icon = Properties.Resources.songfile;

            TextLabel.DoubleClick += (sender, e) =>
            {
                var desc = NoteDescription;
                var splitted = desc.Split("\r");
                if (
                    splitted.Length > 0
                    && Path.IsPathFullyQualified(splitted[0])
                    && File.Exists(splitted[0])
                )
                {
                    Process.Start("explorer.exe", splitted[0]);
                }
            };
            UsedCreationOrder = usedCreationOrder;
        }
    }
}
