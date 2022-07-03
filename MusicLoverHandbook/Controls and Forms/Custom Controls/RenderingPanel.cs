namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public partial class RenderingPanel : Panel
    {
        #region Public Constructors

        public Panel MovingBox;

        public RenderingPanel()
        {
            Margin = Padding = new(0);
            var mainTable = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new(0),
                Padding = new(0)
            };
            mainTable.ColumnStyles.Add(new(SizeType.Percent, 100));
            mainTable.ColumnStyles.Add(new(SizeType.Absolute, 24));
            mainTable.RowStyles.Add(new(SizeType.Percent, 100));

            MovingBox = new Panel() { Dock = DockStyle.Top, Margin = new(0) };
            var movingBoxContainer = new Panel()
            {
                Dock = DockStyle.Fill,
                Margin = new(0),
                Padding = new(0)
            };
            movingBoxContainer.Controls.Add(MovingBox);

            mainTable.Controls.Add(movingBoxContainer, 0, 0);
            mainTable.Controls.Add(
                new CustomHeightScroll(mainTable, MovingBox)
                {
                    Margin = new(4, 0, 0, 0),
                    Padding = new(0),
                    Dock = DockStyle.Fill
                }
            );

            Controls.Add(mainTable);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            var width = MovingBox.Width;
            MovingBox.Dock = DockStyle.None;
            MovingBox.Anchor =
                AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            MovingBox.Width = width;

            MovingBox.Paint += (sender, e) =>
            {
                var h = MovingBox.Controls
                    .Cast<Control>()
                    .Select(x => x.Height)
                    .Concat(new[] { 0 })
                    .Aggregate((c, n) => c + n);
                MovingBox.Height = h > Height ? h : Height;
                //Debug.WriteLine(MovingBox.Height);
                //Debug.WriteLine(Height);
                if (MovingBox.Height <= Height)
                    MovingBox.Location = new(0, 0);
            };
            base.OnHandleCreated(e);
        }

        #endregion Public Constructors
    }
}
