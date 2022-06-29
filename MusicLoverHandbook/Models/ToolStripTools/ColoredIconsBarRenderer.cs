namespace MusicLoverHandbook.Models.ToolStripTools
{
    public class ColoredIconsBarRenderer : ToolStripProfessionalRenderer
    {
        #region Public Constructors

        public ColoredIconsBarRenderer(Color iconsBarColor)
            : base(new CustomProfessionalColorTable(iconsBarColor)) { }

        #endregion Public Constructors
    }
}
