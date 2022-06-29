using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class RenderingPanel : Panel
    {
        #region Public Constructors

        public Panel MovingContentBox;

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

            MovingContentBox = new Panel()
            {
                Dock = DockStyle.Top,
                Margin = new(0)
            };
            var movingBoxContainer = new Panel()
            {
                Dock = DockStyle.Fill,
                Margin = new(0),
                Padding = new(0)
            };
            movingBoxContainer.Controls.Add(MovingContentBox);
            
            mainTable.Controls.Add(movingBoxContainer, 0, 0);
            mainTable.Controls.Add(
                new CustomHeightScroll(mainTable, MovingContentBox)
                {
                    Margin = new(4,0,0,0),
                    Padding = new(0),
                    Dock = DockStyle.Fill
                }
            );

            Controls.Add(mainTable);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            var width = MovingContentBox.Width;
            MovingContentBox.Dock = DockStyle.None;
            MovingContentBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            MovingContentBox.Width = width;

            MovingContentBox.Paint += (sender, e) =>
            {
                var h = MovingContentBox.Controls.Cast<Control>().Select(x => x.Height).Concat(new[] { 0 }).Aggregate((c, n) => c + n);
                MovingContentBox.Height = h > Height ? h : Height;
            };
            base.OnHandleCreated(e);
        }

        public class CustomHeightScroll : Panel
        {
            public Panel? ScrollButton { get; private set; }
            private Control view;
            private Control dynamic;
            private Timer updateTimer;
            private Timer movementTimer;
            public CustomHeightScroll(Control viewingControl, Control dynamicControl) 
            {
                AutoSize = false;
                view = viewingControl;
                dynamic = dynamicControl;
                dynamic.Paint += DynamicControlPainted;
                updateTimer = new() { Interval = 1, Enabled = true };
                updateTimer.Tick += (sender, e) =>
                {
                    if (updateDelay < 0) {updateDelay=10; return; }
                    else if (updateDelay == 0)
                        UpdateScrollSize();
                    updateDelay--;
                };
                movementTimer = new() { Interval=1,Enabled = false };
                movementTimer.Tick += (sender, e) =>
                {
                    if (ScrollButton == null || view.Height>=GetDynamicHeightContentRelated()) return;
                    var currentCursor = Cursor.Position;
                    var currentButtonPos = scrollStart + (Size)(currentCursor - (Size)cursorStart);
                    var yPos = currentButtonPos.Y;
                    yPos = yPos+ScrollButton.Height>Height?Height-ScrollButton.Height:yPos;
                    yPos = yPos<0?0:yPos;
                    ScrollButton.Location = new(ScrollButton.Location.X, yPos);
                    var locationToProgress = (float)yPos / (float)(Height - ScrollButton.Height);
                    Debug.WriteLine(locationToProgress);
                    dynamic.Location = new(0,(int)((float)(view.Height-GetDynamicHeightContentRelated())*locationToProgress));
                };
                
            }
            private float scrollSizeRelatively => (float)(ScrollButton?.Height??Height) / (float)Height;
            protected override void OnHandleCreated(EventArgs e)
            {
                ScrollButton = new Panel()
                {
                    Location = new(0, 0),
                    Size = new(Size.Width, Size.Width),
                    BackColor = ControlPaint.Light(BackColor, -4f)
                };
                ScrollButton.MouseEnter += (sender, e) =>
                {
                    if (scrollSizeRelatively<1)
                        ScrollButton.BackColor = ControlPaint.Light(BackColor, -6f);
                };
                ScrollButton.MouseLeave += (sender, e) =>
                {
                    if ((MouseButtons & MouseButtons.Left)==0&& scrollSizeRelatively < 1)
                        ScrollButton.BackColor = ControlPaint.Light(BackColor, -4f);
                };
                ScrollButton.MouseDown += (sender, e) =>
                {
                    if (scrollSizeRelatively >= 1) return;
                        ScrollButton.BackColor = ControlPaint.Light(BackColor, -8f);

                    cursorStart = Cursor.Position;
                    scrollStart = ScrollButton.Location;
                    movementTimer.Enabled = true;
                };
                ScrollButton.MouseUp += (sender, e) =>
                {
                    movementTimer.Enabled = false;
                    if (scrollSizeRelatively < 1)
                        ScrollButton.BackColor = ControlPaint.Light(BackColor, -4f);

                };
                ScrollButton.SizeChanged += (sender, e) =>
                {
                    if (scrollSizeRelatively < 1)
                        ScrollButton.BackColor = ControlPaint.Light(BackColor, -4f);
                    else
                        ScrollButton.BackColor = ControlPaint.Light(BackColor, -1f);


                };


                UpdateScrollSize();

                Controls.Add(ScrollButton);

                base.OnHandleCreated(e);
            }
            private Point cursorStart;
            private Point scrollStart;
            private void DynamicControlPainted(object? sender,PaintEventArgs e)
            {
                DelayedScrollSizeUpdate();
            }
            private int GetDynamicHeightContentRelated()
            {
                return dynamic.Controls.Cast<Control>().Select(x => x.Height).Concat(new[] { 0 }).Aggregate((c, n) => c + n);
            }
            private int updateDelay = 30;
            private void DelayedScrollSizeUpdate()
            {
                updateDelay = 1;
            }
            private void UpdateScrollSize()
            {
                if (ScrollButton == null) return;

                var dyH = GetDynamicHeightContentRelated();
                var newHeight = view.Height >= dyH ? Height : (int)((float)Height * (float)view.Height / (float)dyH);
                ScrollButton.Size = new(Size.Width,newHeight);
            }
        }

        #endregion Public Constructors
    }
}
