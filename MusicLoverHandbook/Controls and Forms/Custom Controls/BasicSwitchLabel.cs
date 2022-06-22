using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class BasicSwitchLabel : Label
    {
        public bool SpecialState = false;

        public Color BasicBackColor { get; set; }
        public Color SpecialBackColor { get; set; }

        public Color HoveringColor =>
            Color.FromArgb(
                (BasicBackColor.A + SpecialBackColor.A) / 2,
                Color.FromArgb(
                    (BasicBackColor.R + SpecialBackColor.R) / 2,
                    (BasicBackColor.G + SpecialBackColor.G) / 2,
                    (BasicBackColor.B + SpecialBackColor.B) / 2
                )
            );

        public string BasicTooltipText
        {
            get => basicTooltipText;
            set
            {
                basicTooltipText = value;
                OnTooltipTextChanged();
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

        private void SetBackColor()
        {
            BackColor = SpecialState ? SpecialBackColor : BasicBackColor;
        }

        protected virtual void OnTooltipTextChanged()
        {
            toolTip.SetToolTip(this, SpecialState ? SpecialTooltipText : BasicTooltipText);
        }

        public delegate void StateChangedEventHandler(bool IsSpecialState);
        private StateChangedEventHandler? specialStateChanged;
        private string basicTooltipText = "";
        private string specialTooltipText = "";

        public event StateChangedEventHandler SpecialStateChanged
        {
            add => specialStateChanged += value;
            remove => specialStateChanged -= value;
        }

        protected virtual void OnSpecialStateChanged()
        {
            if (specialStateChanged != null)
                specialStateChanged(SpecialState);
        }
    }
}
