using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using static MusicLoverHandbook.Models.Inerfaces.IControlTheme;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControl : UserControl, INoteControl
    {
        protected virtual int sizeS { get; private set; } = 80;
        protected virtual float textSizeRatio { get; private set; } = 0.5f;
        public abstract NoteType NoteType { get; }
        public Image? Icon { get; set; }
        public string NoteText { get => noteText; set {
                TextLabel.Text = value;
                noteText = value; 
            } }
        public string NoteDescription { get; set; }
        ControlCollection INoteControl.Controls => Controls;

        private Color theme;
        private bool isDeleteShown;
        private bool isEditShown;
        private bool isInfoShown;
        private TableLayoutPanel mainTable;
        private string noteText;

        public event ThemeChangeEventHandler ColorChanged;
        public Label TextLabel { get; private set; }
        public Panel IconPanel { get; private set; }
        public ButtonPanel InfoButton { get; private set; }
        public ButtonPanel EditButton { get; private set; }
        public ButtonPanel DeleteButton { get; private set; }
        public SideButtonsPanel SideButtons { get; private set; }
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

        public bool IsDeleteShown
        {
            get => isDeleteShown;
            set
            {
                ToogleViewing(value, DeleteButton);
                isDeleteShown = value;
            }
        }
        public bool IsEditShown
        {
            get => isEditShown;
            set
            {
                ToogleViewing(value, EditButton);
                isEditShown = value;
            }
        }
        public bool IsInfoShown
        {
            get => isInfoShown;
            set
            {
                ToogleViewing(value, InfoButton);
                isInfoShown = value;
            }
        }

        private void ToogleViewing(bool isVis, Control toToggle)
        {
            if (!isVis)
                toToggle.Hide();
            else
                toToggle.Show();
        }

        protected NoteControl(string text, string description)
        {
            BackColor = Color.Transparent;
            SetupColorTheme(NoteType);
            ConstructLayout();
            InitValues(text,description);
        }
        protected virtual void InitValues(string text,string description)
        {
            
            NoteText = text;
            NoteDescription = description;
        }

        public virtual void SetupColorTheme(NoteType type)
        {
            ThemeColor = type.GetColor() ?? Color.Transparent;
        }

        public virtual void ChangeSize(int size)
        {
            sizeS = size;
            ConstructLayout();
        }

        protected virtual void ConstructLayout()
        {
            SuspendLayout();
            Controls.Remove(mainTable);

            var font = FontContainer.Instance.Families[0];
            Font = new Font(font, sizeS * textSizeRatio, GraphicsUnit.Pixel);

            Padding = new Padding(0);

            mainTable = new TableLayoutPanel()
            {
                Padding = new Padding(0),
                Margin = new Padding(0),
                Dock = DockStyle.Top,
                RowCount = 1,
                ColumnCount = 2
            };
            SideButtons = new SideButtonsPanel(DockStyle.Right) { AutoSize = true, };
            var panelIcon = new Panel()
            {
                Padding = new Padding(0),
                Margin = new Padding(0),
                BackColor = Color.Red
            };
            var textPanel = new Panel() { Padding = new Padding(0), Margin = new Padding(0), };

            TextLabel = new Label()
            {
                UseMnemonic = false,
                Padding = new Padding(0),
                Margin = new Padding(0),
                Text = NoteText,
                BackColor = ThemeColor,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };
            InfoButton = new ButtonPanel(ButtonType.Info, 0)
            {
                BackColor = Color.BlueViolet,
                Size = new Size(sizeS, sizeS)
            };
            DeleteButton = new ButtonPanel(ButtonType.Delete, 2)
            {
                BackColor = Color.Red,
                Size = new Size(sizeS, sizeS)
            };
            EditButton = new ButtonPanel(ButtonType.Edit, 1)
            {
                BackColor = Color.Gold,
                Size = new Size(sizeS, sizeS),
            };

            var comboPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0),
                Margin = new Padding(0),
            };

            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, sizeS));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, sizeS));

            textPanel.Dock = DockStyle.Fill;

            textPanel.Controls.Add(TextLabel);

            comboPanel.Controls.Add(textPanel);
            comboPanel.Controls.Add(SideButtons);

            mainTable.Controls.Add(panelIcon, 0, 0);
            mainTable.Controls.Add(comboPanel, 1, 0);

            SideButtons.AddButton(InfoButton);
            SideButtons.AddButton(DeleteButton);
            SideButtons.AddButton(EditButton);

            mainTable.MaximumSize = new Size(0, sizeS);

            mainTable.Controls
                .Cast<Control>()
                .ToList()
                .ForEach(
                    c =>
                    {
                        Padding = new Padding(0);
                    }
                );

            Controls.Add(mainTable);
            Size = new Size(10, sizeS);

            mainTable.Size = new Size(1000, sizeS);
            ResumeLayout();
        }
        public override string ToString()
        {
            return $"{NoteText}";
        }
    }
}
