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
            };
        }

        public ContextMenuStrip MenuStrip { get; set; }
    }
}
