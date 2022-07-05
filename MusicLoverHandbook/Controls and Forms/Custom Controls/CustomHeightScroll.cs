using Timer = System.Windows.Forms.Timer;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class CustomHeightScroll : Panel
    {
        #region Private Fields

        private Point cursorStart;
        private Timer movementTimer;
        private Control movingControl;
        private Point scrollStart;
        private int updateDelay = 30;
        private Timer updateTimer;
        private Control viewControl;

        #endregion Private Fields

        #region Private Properties

        private Panel? scrollButton { get; set; }
        private float scrollSizeRelatively => (float)(scrollButton?.Height ?? Height) / Height;

        #endregion Private Properties

        #region Public Constructors + Destructors

        public CustomHeightScroll(Control viewingControl, Control dynamicControl)
        {
            AutoSize = false;
            viewControl = viewingControl;
            movingControl = dynamicControl;
            movingControl.Paint += DynamicControlPainted;
            updateTimer = new() { Interval = 1, Enabled = true };
            updateTimer.Tick += (sender, e) =>
            {
                if (updateDelay < 0)
                {
                    updateDelay = 10;
                    return;
                }
                else if (updateDelay == 0)
                    UpdateScrollSize();
                updateDelay--;
            };
            movementTimer = new() { Interval = 1, Enabled = false };
            movementTimer.Tick += (sender, e) =>
            {
                if (scrollButton == null || viewControl.Height >= GetDynamicHeightContentRelated())
                    return;
                var currentCursor = Cursor.Position;
                var currentButtonPos = scrollStart + (Size)(currentCursor - (Size)cursorStart);
                var yPos = currentButtonPos.Y;
                yPos = yPos + scrollButton.Height > Height ? Height - scrollButton.Height : yPos;
                yPos = yPos < 0 ? 0 : yPos;
                scrollButton.Location = new(scrollButton.Location.X, yPos);
                var locationToProgress = (float)yPos / (float)(Height - scrollButton.Height);
                //Debug.WriteLine(locationToProgress);
                movingControl.Location = new(
                    0,
                    (int)(
                        (float)(viewControl.Height - GetDynamicHeightContentRelated())
                        * locationToProgress
                    )
                );
            };
        }

        #endregion Public Constructors + Destructors

        #region Protected Methods

        protected override void OnHandleCreated(EventArgs e)
        {
            scrollButton = new Panel()
            {
                Location = new(0, 0),
                Size = new(Size.Width, Size.Width),
                BackColor = ControlPaint.Light(BackColor, -4f)
            };
            scrollButton.MouseEnter += (sender, e) =>
            {
                if (scrollSizeRelatively < 1)
                    scrollButton.BackColor = ControlPaint.Light(BackColor, -6f);
            };
            scrollButton.MouseLeave += (sender, e) =>
            {
                if ((MouseButtons & MouseButtons.Left) == 0 && scrollSizeRelatively < 1)
                    scrollButton.BackColor = ControlPaint.Light(BackColor, -4f);
            };
            scrollButton.MouseDown += (sender, e) =>
            {
                if (scrollSizeRelatively >= 1)
                    return;
                scrollButton.BackColor = ControlPaint.Light(BackColor, -8f);

                cursorStart = Cursor.Position;
                scrollStart = scrollButton.Location;
                movementTimer.Enabled = true;
            };
            scrollButton.MouseUp += (sender, e) =>
            {
                movementTimer.Enabled = false;
                if (scrollSizeRelatively < 1)
                    scrollButton.BackColor = ControlPaint.Light(BackColor, -4f);
            };
            scrollButton.SizeChanged += (sender, e) =>
            {
                if (scrollSizeRelatively < 1)
                    scrollButton.BackColor = ControlPaint.Light(BackColor, -4f);
                else
                    scrollButton.BackColor = ControlPaint.Light(BackColor, -1f);
            };

            UpdateScrollSize();

            Controls.Add(scrollButton);

            base.OnHandleCreated(e);
        }

        #endregion Protected Methods

        #region Private Methods

        private void DelayedScrollSizeUpdate()
        {
            updateDelay = 1;
        }

        private void DynamicControlPainted(object? sender, PaintEventArgs e)
        {
            DelayedScrollSizeUpdate();
        }

        private int GetDynamicHeightContentRelated()
        {
            return movingControl.Controls
                .Cast<Control>()
                .Select(x => x.Height)
                .Concat(new[] { 0 })
                .Aggregate((c, n) => c + n);
        }

        private void UpdateScrollSize()
        {
            if (scrollButton == null)
                return;

            var dyH = GetDynamicHeightContentRelated();
            var newHeight =
                viewControl.Height >= dyH
                    ? Height
                    : (int)((float)Height * (float)viewControl.Height / (float)dyH);
            scrollButton.Size = new(Size.Width, newHeight);
        }

        #endregion Private Methods
    }
}
