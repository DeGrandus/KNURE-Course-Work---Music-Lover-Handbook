using MusicLoverHandbook.Controls_and_Forms.Forms;
using System;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class StripMenuButton : Button
    {
        public StripMenuButton()
        {
            MenuStrip = new ContextMenuStrip();
            Click += (sender, e) =>
            {
                MenuStrip.Font = FindForm().Font ?? DefaultFont;
                MenuStrip.BackColor = ControlPaint.Light(BackColor, 0.5f);
                MenuStrip.Show(this, Location + new Size(0, Height));
                MenuStrip.Renderer = new ColoredIconsBarToolStripRenderer(ImageStripColor??BackColor);
            };
        }
        public Color? ImageStripColor;
        public ContextMenuStrip MenuStrip { get; set; }
    }
}
