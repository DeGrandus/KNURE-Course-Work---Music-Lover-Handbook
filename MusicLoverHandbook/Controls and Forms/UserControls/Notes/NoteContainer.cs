using MusicLoverHandbook.Controls_and_Forms.Forms;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls.Notes
{
    public partial class NoteContainer : UserControl
    {
        public MainForm MainForm { get; }
        public Panel NotesPanel { get; }

        public NoteContainer(MainForm form)
        {
            InitializeComponent();
            MainForm = form;

            NotesPanel = new Panel() { Dock = DockStyle.Fill, AutoScroll = true };
            Controls.Add(NotesPanel);
        }
    }
}
