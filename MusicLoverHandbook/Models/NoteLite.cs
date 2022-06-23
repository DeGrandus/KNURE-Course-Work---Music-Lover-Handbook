using MusicLoverHandbook.Models.Abstract;
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
        public Image? Icon { get; }
        public NoteControl Ref { get;  }
        public int MainHeight { get; set; } = 20;

        public NoteLite(string name, string description, NoteControl noteRef)
        {
            NoteName = name;
            Description = description;
            Icon = noteRef.Icon;
            Ref = noteRef;
            SetupLayout();
        }
        private void SetupLayout()
        {
            SuspendLayout();
            Controls.Clear();

            var mainTable = new TableLayoutPanel()
            {
                Margin = new(0),
                Padding = new(0),
                Dock = DockStyle.Top,
                Height = 20,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Ref.ThemeColor
            };
            mainTable.ColumnStyles.Add(new(SizeType.Absolute, MainHeight));
            mainTable.ColumnStyles.Add(new(SizeType.Percent, 100));
            mainTable.ColumnStyles.Add(new(SizeType.Absolute, MainHeight));

            var iconPanel = new Panel()
            {
                Margin = new(0),
                Padding = new(0),
                BackgroundImage = Icon,
                BackColor = ControlPaint.Light(Ref.ThemeColor),
                Dock=DockStyle.Fill,
            };
            var infoPanel = new Panel()
            {
                Margin = new(0),
                Padding = new(0),
                BackgroundImage = Properties.Resources.info,
                BackColor = ControlPaint.Light(Ref.ThemeColor),
                Dock = DockStyle.Fill,
            };
            var tooltip = new ToolTip()
            {
                BackColor = ControlPaint.Light(Ref.ThemeColor),
                OwnerDraw = true
            };
            tooltip.Draw += (sender, e) =>
            {
                e.DrawBackground();
                e.DrawBorder();
                e.DrawText();
            };
            tooltip.SetToolTip(infoPanel, Description);
            var nameLabel = new Label()
            {
                Margin = new(0),
                Padding = new(0),
                BackColor = Ref.ThemeColor,
                Dock = DockStyle.Fill,
                Text = NoteName
            };

            mainTable.Controls.Add(iconPanel,0,0);
            mainTable.Controls.Add(nameLabel,1,0);
            mainTable.Controls.Add(infoPanel,2,0);


            ResumeLayout();
        }
    }
}
