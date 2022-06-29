namespace MusicLoverHandbook.Models.Attributes
{
    public class EnumColorAttribute : Attribute
    {
        #region Public Properties

        public Color? ColorLite { get; }

        public Color ColorMain { get; }

        #endregion Public Properties

        #region Public Constructors

        public EnumColorAttribute(int alphaMain, int colorMain, int alphaLite, int colorLite)
            : this(alphaMain, colorMain)
        {
            ColorLite = Color.FromArgb(alphaLite, Color.FromArgb(colorLite));
        }

        public EnumColorAttribute(int alphaMain, int colorMain)
        {
            ColorMain = Color.FromArgb(alphaMain, Color.FromArgb(colorMain));
            ColorLite = null;
        }

        #endregion Public Constructors
    }
}