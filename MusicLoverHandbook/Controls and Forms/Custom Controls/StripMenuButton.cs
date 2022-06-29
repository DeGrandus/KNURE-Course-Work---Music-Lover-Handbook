using MusicLoverHandbook.Models;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class StripMenuButton : Button
    {
        #region Public Fields

        public Color? ImageStripColor;

        #endregion Public Fields



        #region Public Properties

        public ContextMenuStrip MenuStrip { get; set; }

        #endregion Public Properties



        #region Public Constructors

        public StripMenuButton()
        {
            MenuStrip = new ContextMenuStrip();
            Click += (sender, e) =>
            {
                MenuStrip.Font = FindForm().Font ?? DefaultFont;
                MenuStrip.BackColor = ControlPaint.Light(BackColor, 0.5f);
                MenuStrip.Show(this, Location + new Size(0, Height));
                MenuStrip.Renderer = new ColoredIconsBarRenderer(ImageStripColor ?? BackColor);
            };
        }

        #endregion Public Constructors
    }
}
