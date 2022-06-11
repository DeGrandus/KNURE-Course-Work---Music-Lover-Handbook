using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using static MusicLoverHandbook.Models.Inerfaces.IControlTheme;

namespace MusicLoverHandbook.Models
{
    public abstract class NoteControl : UserControl, INoteControl, IControlTheme
    {
        protected static int sizeS = 50;
        public abstract NoteType Type { get; }
        public Image? Icon { get; set; }
        public string NoteText { get; set; }
        public string NoteDescription { get; set; }
        ControlCollection INoteControl.Controls => Controls;

        private Color theme;

        public event ThemeChangeEventHandler ColorChanged;
        public Label TextLabel { get; set; }
        public Color ThemeColor
        {
            get => theme;
            set
            {
                theme = value;
                OnColorChanged();
            }
        }

        public void OnColorChanged()
        {
            if (ColorChanged != null)
                ColorChanged(this, new(ThemeColor));
        }

        protected NoteControl(string text, string description)
        {
            NoteText = text;
            NoteDescription = description;
            ThemeColor = Type.GetColor();

            BackColor = Color.Transparent;

            ConstructLayout();
        }

        protected void ConstructLayout()
        {
            SuspendLayout();

            var font = FontContainer.Instance.Families[0];
            Font = new Font(font, sizeS, FontStyle.Bold, GraphicsUnit.Pixel);

            var table = new TableLayoutPanel()
            {
                Dock = DockStyle.Top,
                RowCount = 1,
                ColumnCount = 5
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, sizeS));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, sizeS));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, sizeS));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, sizeS));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, sizeS));
            var panelIcon = new Panel() { BackColor = Color.Red };
            var text = new Panel();
            text.Dock = DockStyle.Fill;
            TextLabel = new Label()
            {
                Text = NoteText,
                BackColor = ThemeColor,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };
            text.Controls.Add(TextLabel);
            var desc = new Panel() { BackColor = Color.Blue };
            var del = new Panel() { BackColor = Color.Green };
            var edit = new Panel() { BackColor = Color.Red };

            table.Controls.Add(panelIcon, 0, 0);
            table.Controls.Add(text, 1, 0);
            table.Controls.Add(desc, 2, 0);
            table.Controls.Add(del, 3, 0);
            table.Controls.Add(edit, 4, 0);

            table.MaximumSize = new Size(0, sizeS);

            table.Controls
                .Cast<Control>()
                .ToList()
                .ForEach(
                    c =>
                    {
                        c.Dock = DockStyle.Fill;
                        c.Margin = new Padding(0);
                    }
                );

            Controls.Add(table);
            Size = new Size(10, sizeS);

            table.Size = new Size(1000, sizeS);
            ResumeLayout();
        }
    }
}
