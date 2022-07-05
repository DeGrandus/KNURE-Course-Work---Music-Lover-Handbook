namespace MusicLoverHandbook.Models.ToolStripTools
{
    public class ColoredIconsBarRenderer : ToolStripProfessionalRenderer
    {
        #region Public Constructors + Destructors

        public ColoredIconsBarRenderer(Color iconsBarColor)
            : base(new CustomProfessionalColorTable(iconsBarColor)) { }

        #endregion Public Constructors + Destructors
    }
}
