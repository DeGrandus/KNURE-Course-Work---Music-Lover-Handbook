using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Models.Managers;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class NoteSongFile : NoteControlChild
    {
        #region Public Constructors + Destructors

        public NoteSongFile(
            IParentControl song,
            string text,
            string description,
            NoteCreationOrder? usedCreationOrder
        ) : base(song, text, description, NoteType.SongFile, usedCreationOrder)
        {
            InitializeComponent();

            Icon = Properties.Resources.SongFileIcon;

            TextLabel.DoubleClick += (sender, e) =>
            {
                var desc = NoteDescription;
                var splitted = Regex.Replace(desc, @"(\r|\n)+", "\r\n").Split("\r\n");
                if (
                    splitted.Length > 0
                    && (
                        (Path.IsPathRooted(splitted[0]) && File.Exists(splitted[0]))
                        || FileManager.Instance.GetMusicFilePathByName(splitted[0]) is string
                    )
                )
                {
                    Process.Start(
                        "explorer.exe",
                        FileManager.Instance.GetMusicFilePathByName(splitted[0]) ?? splitted[0]
                    );
                }
            };
        }

        #endregion Public Constructors + Destructors
    }
}
