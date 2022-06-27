namespace MusicLoverHandbook.Models
{
    public class ColoredIconsBarToolStripRenderer : ToolStripProfessionalRenderer
    {
        public ColoredIconsBarToolStripRenderer(Color color)
            : base(new CustomProfessionalColorTable(color)) { }
    }
}
