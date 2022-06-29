namespace MusicLoverHandbook.View.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }
        protected override void OnLostFocus(EventArgs e)
        {
            Close();
            base.OnLostFocus(e);
        }

    }
}
