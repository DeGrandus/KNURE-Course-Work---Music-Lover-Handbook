namespace MusicLoverHandbook.Models
{
    public class CustomProfessionalColorTable : ProfessionalColorTable
    {
        private Color imageStripColor;

        public override Color ImageMarginGradientBegin => imageStripColor;

        public override Color ImageMarginGradientEnd => imageStripColor;

        public override Color ImageMarginRevealedGradientMiddle => imageStripColor;

        public CustomProfessionalColorTable(Color imageStripColor)
        {
            this.imageStripColor = imageStripColor;
        }
    }
}
