using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models
{
    public class NoteLite : Control
    {
        public string NoteName { get; }
        public string Description { get; }
        public NoteType NoteType { get; }
        public Image? Icon { get; }
        public NoteControl Ref { get; }
        public int MainHeight { get; set; } = 30;
        public int ID { get; set; }

        public NoteLite(string name, string description, NoteControl noteRef)
        {
            NoteName = name;
            Description = description;
            NoteType = noteRef.NoteType;
            Icon = noteRef.Icon;
            Ref = noteRef;
            ID = Random.Shared.Next(int.MinValue, int.MaxValue);
            SetupLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            base.OnPaint(e);
        }

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
                BackColor = Ref.ThemeColor
            };
            mainTable.ColumnStyles.Add(new(SizeType.Absolute, MainHeight));
            mainTable.ColumnStyles.Add(new(SizeType.Percent, 100));
            mainTable.ColumnStyles.Add(new(SizeType.Absolute, MainHeight));
            mainTable.RowStyles.Add(new(SizeType.Absolute, MainHeight));

            var iconPanel = new Panel()
            {
                Margin = new(0),
                Padding = new(0),
                BackColor = Ref.NoteType.GetLiteColor() ?? Ref.ThemeColor,
                Dock = DockStyle.Fill,
                BackgroundImage = Icon,
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            var infoPanel = new Panel()
            {
                Margin = new(0),
                Padding = new(0),
                BackgroundImage = Properties.Resources.info,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Ref.NoteType.GetLiteColor() ?? Ref.ThemeColor,
                Dock = DockStyle.Fill,
            };
            var tooltip = new ToolTip()
            {
                BackColor = ControlPaint.Light(Ref.NoteType.GetLiteColor() ?? Ref.ThemeColor),
                IsBalloon = true,
                InitialDelay = 50,
            };
            tooltip.SetToolTip(infoPanel, Description);
            var nameLabel = new Label()
            {
                Margin = new(0),
                Padding = new(0),
                BackColor = Ref.NoteType.GetLiteColor() ?? Ref.ThemeColor,
                Dock = DockStyle.Fill,
                Text = NoteName,
                Font = new Font(Ref.TextLabel.Font.FontFamily, MainHeight, GraphicsUnit.Pixel)
            };

            mainTable.Controls.Add(iconPanel, 0, 0);
            mainTable.Controls.Add(nameLabel, 1, 0);
            mainTable.Controls.Add(infoPanel, 2, 0);

            Size = new(10, MainHeight);
            Controls.Add(mainTable);
            BackColor = Color.White;

            ResumeLayout();
        }
        public NoteLite Clone()
        {
            return new NoteLite(NoteName,Description,Ref);
        }
        public override string ToString()
        {
            return $@"Lite: {{Name: {NoteName} | Desc: {Description} | Type: {NoteType}}}";
        }
        public override bool Equals(object? obj)
        {
            return obj is NoteLite lite &&
                   NoteName == lite.NoteName &&
                   Description == lite.Description &&
                   NoteType == lite.NoteType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NoteName, Description, NoteType);
        }
    }
}
