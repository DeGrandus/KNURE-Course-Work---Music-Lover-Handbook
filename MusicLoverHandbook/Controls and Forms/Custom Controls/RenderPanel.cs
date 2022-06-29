using System.Diagnostics;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class RenderingPanel : Panel
    {
        #region Public Constructors

        public Panel MovingContentBox;
        public RenderingPanel()
        {
            Margin = Padding = new(0);
            var mainTable = new TableLayoutPanel() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Margin = new(0),Padding=new(0) };
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute,24));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent,100));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute,24));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));

            MovingContentBox = new Panel() { Dock=DockStyle.Top,AutoSize=true,BackColor=Color.White,Margin=new(0)};
            mainTable.Controls.Add(MovingContentBox,0,1);

            var buttons = (up: new Button(), down: new Button());
            HandleCreated += (sender, e) =>
            {
                foreach (var b in new[] { buttons.up, buttons.down })
                {
                    b.Dock = DockStyle.Fill;
                    b.Margin = b.Padding = new(0);
                    b.FlatStyle = FlatStyle.Flat;
                    b.BackColor = ControlPaint.Light(BackColor, -3f);
                    b.FlatAppearance.BorderSize = 0;
                    b.FlatAppearance.MouseDownBackColor = ControlPaint.Light(BackColor, -8f);
                };
            };
            buttons.up.MouseDown += (sender, e) => Debug.WriteLine("TEst");
            buttons.up.MouseUp += (sender, e) => Debug.WriteLine("TEst2");

            mainTable.Controls.Add(buttons.up, 0, 0);
            mainTable.Controls.Add(buttons.down, 0, 2);

            Controls.Add(mainTable);

            
        }
        

        #endregion Public Constructors
    }
}
