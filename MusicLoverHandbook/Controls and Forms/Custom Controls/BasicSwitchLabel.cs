namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class BasicSwitchLabel : Label
    {
        public bool SpecialState
        {
            get => specialState; set
            {
                specialState = value;
                SetBackColor();
                OnTooltipTextChanged();
                OnSpecialStateChanged();
            }
        }

        private string basicTooltipText = "";
        private StateChangedEventHandler? specialStateChanged;
        private string specialTooltipText = "";
        private ToolTip toolTip;
        private bool specialState = false;

        public BasicSwitchLabel()
        {
            toolTip = new ToolTip();
            HandleCreated += OnHandleCreated;
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
                if (SwitchType == SwitchMode.DoubleClick)
                    SpecialState = !SpecialState;
            };
            Click += (sender, e) =>
            {
                if (SwitchType==SwitchMode.Click)
                    SpecialState = !SpecialState;
            };
        }
        public enum SwitchMode
        {
            Click,DoubleClick
        }
        public SwitchMode SwitchType { get; set; } = SwitchMode.DoubleClick;
        private bool initialState = false;
        private bool colorsInited = false;
        private void OnHandleCreated(object? sender, EventArgs e)
        {
            if (!colorsInited)
            {
                BasicBackColor = Parent.BackColor;
                SpecialBackColor = ControlPaint.Light(BasicBackColor, -2f);
            }
            SpecialState = initialState;
        }
        public BasicSwitchLabel(bool initialState) : this()
        {
            this.initialState = initialState;
        }
        public BasicSwitchLabel(Color basicBackColor, Color specialBackColor) : this(false)
        {
            colorsInited = true;
            BasicBackColor = basicBackColor;
            SpecialBackColor = specialBackColor;
        }
        public BasicSwitchLabel(Color basicBackColor, Color specialBackColor, bool initialState) : this(basicBackColor,specialBackColor)
        {
            this.initialState = initialState;
        }

        public delegate void StateChangedEventHandler(bool IsSpecialState);

        public event StateChangedEventHandler SpecialStateChanged
        {
            add => specialStateChanged += value;
            remove => specialStateChanged -= value;
        }



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
        public Color BasicBackColor { get; set; }
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
