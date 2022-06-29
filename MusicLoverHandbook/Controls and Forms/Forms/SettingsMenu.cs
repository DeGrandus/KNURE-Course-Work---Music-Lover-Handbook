using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Models;

namespace MusicLoverHandbook.View.Forms
{
    public partial class SettingsMenu : Form
    {
        #region Public Constructors

        public SettingsMenu(MainForm mainForm)
        {
            InitializeComponent();

            Font = mainForm.Font;
            titleLabel.BackColor = mainForm.title.BackColor;
            titleLabel.Font = mainForm.title.Font;
            MinimumSize = new Size(1000, 700);
            Size = MinimumSize;
            mainTable.Controls
                .OfType<Button>()
                .ToList()
                .ForEach(b => b.BackColor = ControlPaint.Light(mainForm.title.BackColor));
            mainTable.Controls
                .OfType<TextBox>()
                .ToList()
                .ForEach(
                    tb =>
                    {
                        tb.TextAlign = HorizontalAlignment.Center;
                        tb.BackColor = Color.White;
                    }
                );
            currentDataPathField.Text = FileManager.Instance.DataFilePath;
            currentMusicFilesFolderField.Text = FileManager.Instance.MusicFilesFolderPath;

            resetDataFilePathButton.Click += (sender, e) =>
            {
                FileManager.Instance.ResetDataFilePathToDefault();
                currentDataPathField.Text = FileManager.Instance.DataFilePath;
            };
            resetMusicFilesFolderPathButton.Click += (sender, e) =>
            {
                FileManager.Instance.ResetMusicFilesFolderPathToDefalut();
                currentMusicFilesFolderField.Text = FileManager.Instance.MusicFilesFolderPath;
            };
            setNewMusicFilesFolderPathButton.Click += (sender, e) =>
            {
                var selectFolderMenu = new FolderBrowserDialog();
                var result = selectFolderMenu.ShowDialog();
                if (result == DialogResult.OK)
                    FileManager.Instance.SetMusicFilesFolderPath(selectFolderMenu.SelectedPath);
                currentMusicFilesFolderField.Text = FileManager.Instance.MusicFilesFolderPath;
            };
            saveOnCloseCheck.Checked = FileManager.Instance.SaveOnClose;
            saveOnCloseCheck.CheckedChanged += (sender, e) =>
            {
                FileManager.Instance.SaveOnClose = saveOnCloseCheck.Checked;
            };
        }

        #endregion Public Constructors
    }
}
