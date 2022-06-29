namespace MusicLoverHandbook.Models
{
    public class ColoredIconsBarRenderer : ToolStripProfessionalRenderer
    {
        public ColoredIconsBarRenderer(Color iconsBarColor)
            : base(new CustomProfessionalColorTable(iconsBarColor)) { }
    }
}
