namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class BasicSwitchLabel : Label
    {
        public bool InitialState = false;

        private string basicTooltipText = "";

        private bool colorsInited = false;

        private bool specialState = false;

        private StateChangedEventHandler? specialStateChanged;

        private string specialTooltipText = "";

        private ToolTip toolTip;

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

        public string SpecialTooltipText
        {
            get => specialTooltipText;
            set
            {
                specialTooltipText = value;
                OnTooltipTextChanged();
            }
        }

        public SwitchMode SwitchType { get; set; } = SwitchMode.DoubleClick;

        public BasicSwitchLabel()
        {
            toolTip = new ToolTip()
            {
                IsBalloon = true,
            };
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
                if (SwitchType == SwitchMode.Click)
                    SpecialState = !SpecialState;
            };
        }

        public BasicSwitchLabel(bool initialState) : this()
        {
            this.InitialState = initialState;
        }

        public BasicSwitchLabel(Color basicBackColor, Color specialBackColor) : this(false)
        {
            colorsInited = true;
            BasicBackColor = basicBackColor;
            SpecialBackColor = specialBackColor;
        }

        public BasicSwitchLabel(Color basicBackColor, Color specialBackColor, bool initialState) : this(basicBackColor, specialBackColor)
        {
            this.InitialState = initialState;
        }

        public int? GetEventCount() => specialStateChanged?.GetInvocationList().Length;

        protected virtual void OnSpecialStateChanged()
        {
            if (specialStateChanged != null)
                specialStateChanged(this, SpecialState);
        }

        protected virtual void OnTooltipTextChanged()
        {
            toolTip.SetToolTip(this, SpecialState ? SpecialTooltipText : BasicTooltipText);
        }

        private void OnHandleCreated(object? sender, EventArgs e)
        {
            if (!colorsInited)
            {
                BasicBackColor = Parent.BackColor;
                SpecialBackColor = ControlPaint.Light(BasicBackColor, -2f);
            }
            SpecialState = InitialState;
        }

        private void SetBackColor()
        {
            BackColor = SpecialState ? SpecialBackColor : BasicBackColor;
        }

        public delegate void StateChangedEventHandler(object? sender, bool IsSpecialState);

        public event StateChangedEventHandler SpecialStateChanged
        {
            add => specialStateChanged += value;
            remove => specialStateChanged -= value;
        }

        public enum SwitchMode
        {
            Click, DoubleClick
        }
    }
}