using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using System.ComponentModel;

namespace MusicLoverHandbook.Models.NoteAlter
{
    [DesignerCategory("Code")]
    public class LiteNote : Control
    {
        #region Public Properties

        public Image? Icon { get; }
        public int MainHeight { get; set; } = 30;
        public string NoteDescription { get; }
        public string NoteName { get; }
        public NoteType NoteType { get; }
        public NoteControl OriginalNoteRefference { get; }

        #endregion Public Properties

        #region Public Constructors

        public LiteNote(string name, string description, NoteControl noteRef)
        {
            NoteName = name;
            NoteDescription = description;
            NoteType = noteRef.NoteType;
            Icon = noteRef.Icon;
            OriginalNoteRefference = noteRef;
            SetupLayout();
        }

        #endregion Public Constructors

        #region Public Methods

        public LiteNote Clone()
        {
            return new LiteNote(NoteName, NoteDescription, OriginalNoteRefference);
        }

        public override bool Equals(object? obj)
        {
            return obj is LiteNote lite
                && NoteName == lite.NoteName
                && NoteDescription == lite.NoteDescription
                && NoteType == lite.NoteType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NoteName, NoteDescription, NoteType);
        }

        public override string ToString()
        {
            return $@"Lite: {{Name: {NoteName} | Desc: {NoteDescription} | Type: {NoteType}}}";
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            base.OnPaint(e);
        }

        #endregion Protected Methods

        #region Private Methods

        private void SetupLayout()
        {
            SuspendLayout();
            Controls.Clear();

            var mainTable = new TableLayoutPanel()
            {
                Margin = new(0),
                Padding = new(0),
                Dock = DockStyle.Fill,
                Height = MainHeight,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = OriginalNoteRefference.ThemeColor
            };
            mainTable.ColumnStyles.Add(new(SizeType.Absolute, MainHeight));
            mainTable.ColumnStyles.Add(new(SizeType.Percent, 100));
            mainTable.ColumnStyles.Add(new(SizeType.Absolute, MainHeight));
            mainTable.RowStyles.Add(new(SizeType.Absolute, MainHeight));

            var iconPanel = new Panel()
            {
                Margin = new(0),
                Padding = new(0),
                BackColor =
                    OriginalNoteRefference.NoteType.GetLiteColor()
                    ?? OriginalNoteRefference.ThemeColor,
                Dock = DockStyle.Fill,
                BackgroundImage = Icon,
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            var infoPanel = new Panel()
            {
                Margin = new(0),
                Padding = new(0),
                BackgroundImage = Properties.Resources.InfoIcon,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor =
                    OriginalNoteRefference.NoteType.GetLiteColor()
                    ?? OriginalNoteRefference.ThemeColor,
                Dock = DockStyle.Fill,
            };
            var tooltip = new ToolTip()
            {
                BackColor = ControlPaint.Light(
                    OriginalNoteRefference.NoteType.GetLiteColor()
                        ?? OriginalNoteRefference.ThemeColor
                ),
                IsBalloon = true,
                InitialDelay = 50,
            };
            tooltip.SetToolTip(infoPanel, NoteDescription);
            var nameLabel = new Label()
            {
                Margin = new(0),
                Padding = new(0),
                BackColor =
                    OriginalNoteRefference.NoteType.GetLiteColor()
                    ?? OriginalNoteRefference.ThemeColor,
                Dock = DockStyle.Fill,
                Text = NoteName,
                Font = new Font(
                    OriginalNoteRefference.TextLabel.Font.FontFamily,
                    MainHeight,
                    GraphicsUnit.Pixel
                )
            };

            mainTable.Controls.Add(iconPanel, 0, 0);
            mainTable.Controls.Add(nameLabel, 1, 0);
            mainTable.Controls.Add(infoPanel, 2, 0);

            Size = new(10, MainHeight);
            Controls.Add(mainTable);
            BackColor = Color.White;

            ResumeLayout();
        }

        #endregion Private Methods
    }
}