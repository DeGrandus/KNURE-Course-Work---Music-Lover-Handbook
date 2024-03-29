﻿namespace MusicLoverHandbook.Models.ToolStripTools
{
    public class CustomProfessionalColorTable : ProfessionalColorTable
    {
        #region Private Fields

        private Color imageStripColor;

        #endregion Private Fields

        #region Public Properties

        public override Color ImageMarginGradientBegin => imageStripColor;

        public override Color ImageMarginGradientEnd => imageStripColor;

        public override Color ImageMarginRevealedGradientMiddle => imageStripColor;

        #endregion Public Properties

        #region Public Constructors + Destructors

        public CustomProfessionalColorTable(Color imageStripColor)
        {
            this.imageStripColor = imageStripColor;
        }

        #endregion Public Constructors + Destructors
    }
}
