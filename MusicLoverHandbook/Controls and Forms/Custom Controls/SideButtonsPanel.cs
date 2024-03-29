﻿using MusicLoverHandbook.Models.Enums;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class SideButtonsPanel : Panel
    {
        #region Private Fields

        private List<ButtonPanel> activatedButtons = new();
        private List<ButtonPanel> deactivatedButtons = new();
        private DockStyle dockStyle;

        #endregion Private Fields

        #region Public Properties

        public IReadOnlyCollection<ButtonPanel> ActiveButtons => activatedButtons;

        private new ControlCollection Controls => base.Controls;

        public IReadOnlyCollection<ButtonPanel> DeactivatedButtons => deactivatedButtons;

        #endregion Public Properties

        #region Public Constructors + Destructors

        public SideButtonsPanel(DockStyle dock)
        {
            Margin = Padding = new Padding(0);
            Dock = dockStyle = dock;
        }

        #endregion Public Constructors + Destructors

        #region Public Methods

        public void Activate(ButtonType type)
        {
            if (
                deactivatedButtons.Where(x => x.ButtonType == type).FirstOrDefault()
                is ButtonPanel button
            )
            {
                Controls.Add(button);
                deactivatedButtons.Remove(button);
                activatedButtons.Add(button);
                ReorganizeControls();
            }
        }

        public void AddButton(ButtonPanel button)
        {
            if (activatedButtons.Concat(deactivatedButtons).ToList().Find(x => x == button) == null)
            {
                button.Dock = dockStyle;
                activatedButtons.Add(button);
                Controls.Add(button);
                ReorganizeControls();
            }
        }

        public void Deactivate(ButtonType type)
        {
            if (
                activatedButtons.Where(x => x.ButtonType == type).FirstOrDefault()
                is ButtonPanel button
            )
            {
                Controls.Remove(button);
                deactivatedButtons.Add(button);
                activatedButtons.Remove(button);
                ReorganizeControls();
            }
        }

        public void RemoveButton(ButtonPanel button)
        {
            if (activatedButtons.Find(x => x == button) != null)
            {
                activatedButtons.Remove(button);
                Controls.Remove(button);
            }
            if (deactivatedButtons.Find(x => x == button) != null)
                deactivatedButtons.Remove(button);
        }

        public void ReorganizeControls()
        {
            foreach (var pButton in Controls.Cast<Control>())
            {
                if (pButton is ButtonPanel buttonPanel)
                {
                    Controls.SetChildIndex(buttonPanel, buttonPanel.OrderIndex);
                }
            }
        }

        #endregion Public Methods
    }
}
