namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class BasicSwitchLabel : Label
    {
        public bool SpecialState = false;

        private string basicTooltipText = "";
        private StateChangedEventHandler? specialStateChanged;
        private string specialTooltipText = "";
        private ToolTip toolTip;

        public BasicSwitchLabel()
        {
            toolTip = new ToolTip();
            HandleCreated += (sender, e) =>
            {
                BasicBackColor = Parent.BackColor;
                SpecialBackColor = ControlPaint.Light(BasicBackColor, -2f);
            };
            MouseEnter += (sender, e) =>
            {
                BackColor = HoveringColor;
            };
            MouseLeave += (sender, e) =>
            {
                SetBackColor();
            };
            MouseDown += (sender, e) =>
            {
                BackColor = ControlPaint.Light(HoveringColor, -0.5f);
            };
            MouseUp += (sender, e) =>
            {
                BackColor = HoveringColor;
            };
            DoubleClick += (sender, e) =>
            {
                SpecialState = !SpecialState;
                SetBackColor();
                OnTooltipTextChanged();
                OnSpecialStateChanged();
            };
        }

        public delegate void StateChangedEventHandler(bool IsSpecialState);

        public event StateChangedEventHandler SpecialStateChanged
        {
            add => specialStateChanged += value;
            remove => specialStateChanged -= value;
        }

        public Color BasicBackColor { get; set; }

        public string BasicTooltipText
        {
            get => basicTooltipText;
            set
            {
                basicTooltipText = value;
                OnTooltipTextChanged();
            }
        }

        public Color HoveringColor =>
            Color.FromArgb(
                (BasicBackColor.A + SpecialBackColor.A) / 2,
                Color.FromArgb(
                    (BasicBackColor.R + SpecialBackColor.R) / 2,
                    (BasicBackColor.G + SpecialBackColor.G) / 2,
                    (BasicBackColor.B + SpecialBackColor.B) / 2
                )
            );

        public Color SpecialBackColor { get; set; }

        public string SpecialTooltipText
        {
            get => specialTooltipText;
            set
            {
                specialTooltipText = value;
                OnTooltipTextChanged();
            }
        }

        protected virtual void OnSpecialStateChanged()
        {
            if (specialStateChanged != null)
                specialStateChanged(SpecialState);
        }

        protected virtual void OnTooltipTextChanged()
        {
            toolTip.SetToolTip(this, SpecialState ? SpecialTooltipText : BasicTooltipText);
        }

        private void SetBackColor()
        {
            BackColor = SpecialState ? SpecialBackColor : BasicBackColor;
        }
    }
}