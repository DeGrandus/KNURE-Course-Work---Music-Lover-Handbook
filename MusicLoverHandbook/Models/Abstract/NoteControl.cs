using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Collections.ObjectModel;
using static MusicLoverHandbook.Models.Inerfaces.IControlTheme;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControl : UserControl, INoteControl
    {
        protected virtual int sizeS { get; private set; } = 70;
        protected virtual float textSizeRatio { get; private set; } = 0.5f;
        public abstract NoteType NoteType { get; }
        public Image? Icon
        {
            get => IconPanel.BackgroundImage; set
            {
                IconPanel.BackgroundImageLayout = ImageLayout.Stretch;
                IconPanel.BackgroundImage = value;
            }
        }

        public string NoteName
        {
            get => noteText;
            set
            {
                TextLabel.Text = value;
                noteText = value;
            }
        }
        public string NoteDescription
        {
            get => noteDescription; set
            {
                if (InfoButton != null) ballonTip?.SetToolTip(InfoButton, value);
                noteDescription = value;
            }
        }
        ControlCollection INoteControl.Controls => Controls;

        private Color theme;
        private bool isDeleteShown;
        private bool isEditShown;
        private bool isInfoShown;
        private TableLayoutPanel mainTable;
        private string noteText;
        private ToolTip ballonTip;
        private string noteDescription;

        public event ThemeChangeEventHandler ColorChanged;
        public Label TextLabel { get; private set; }
        public Panel IconPanel { get; private set; }
        public ButtonPanel InfoButton { get; private set; }
        public ButtonPanel EditButton { get; private set; }
        public ButtonPanel DeleteButton { get; private set; }
        public SideButtonsPanel SideButtons { get; private set; }

        private List<Control?> toCustomizate;
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
            InitCustomLayout();
            InitValues(text, description);
        }

        protected virtual void InitValues(string text, string description)
        {
            NoteName = text;
            NoteDescription = description;
        }

        public virtual void SetupColorTheme(NoteType type)
        {
            ThemeColor = type.GetColor() ?? Color.Transparent;
        }

        public virtual void ChangeSize(int size)
        {
            sizeS = size;
            InitCustomLayout();
        }
        private void OnButtonMouseEnter(object? sender, EventArgs e)
        {
            if (sender is Control control)
                control.BackColor = ControlPaint.Light(control.BackColor,0.4f);

        }
        private void OnButtonMouseLeave(object? sender, EventArgs e)
        {
            if (sender is Control control)
                control.BackColor = ControlPaint.Light(control.BackColor, -0.4f);

        }
        private void InitCustomization()
        {
            foreach(var control in new Control[] { TextLabel,InfoButton,DeleteButton,EditButton })
            {
                control.MouseEnter+=OnButtonMouseEnter;
                control.MouseLeave+=OnButtonMouseLeave;
            } 
        }
        protected virtual void InitCustomLayout()
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
            IconPanel = new Panel()
            {
                Padding = new Padding(0),
                Margin = new Padding(0),
                BackColor = ControlPaint.Light(ThemeColor)
            };

            var textPanel = new Panel() { Padding = new Padding(0), Margin = new Padding(0), };
            ballonTip = new ToolTip();
            ballonTip.IsBalloon = true;
            ballonTip.UseFading = true;
            ballonTip.UseAnimation = true;
            ballonTip.ToolTipIcon = ToolTipIcon.Info;
            ballonTip.ToolTipTitle = "Description";
            ballonTip.InitialDelay = 100;

            TextLabel = new Label()
            {
                UseMnemonic = false,
                Padding = new Padding(0),
                Margin = new Padding(0),
                Text = NoteName,
                BackColor = ThemeColor,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };

            InfoButton = new ButtonPanel(ButtonType.Info, 0)
            {
                BackColor = ControlPaint.Light(ThemeColor),
                BackgroundImageLayout = ImageLayout.Stretch,
                BackgroundImage = Properties.Resources.info,
                Size = new Size(sizeS, sizeS)
            };
            ballonTip.SetToolTip(InfoButton, NoteDescription);
            DeleteButton = new ButtonPanel(ButtonType.Delete, 2)
            {
                BackColor = ControlPaint.Light(ThemeColor),
                BackgroundImageLayout = ImageLayout.Stretch,
                BackgroundImage = Properties.Resources.delete,
                Size = new Size(sizeS, sizeS)
            };
            DeleteButton.Click += (sender, e) =>
            {
                if (this is INoteControlChild asChild)
                {
                    var box = MessageBox.Show($"Are you sure you want to delete {NoteType} {NoteName}?", "Delete warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (box == DialogResult.Yes)
                        if (asChild.ParentNote is INoteControlParent asParent) asParent.Linker.Observed.Remove(asChild);
                        else asChild.ParentNote.InnerNotes.Remove(asChild);
                }

            };
            EditButton = new ButtonPanel(ButtonType.Edit, 1)
            {
                BackColor = ControlPaint.Light(ThemeColor),
                BackgroundImageLayout = ImageLayout.Stretch,
                BackgroundImage = Properties.Resources.edit,
                Size = new Size(sizeS, sizeS),
            };
            EditButton.Click += (sender, e) => EditClick();

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

            mainTable.Controls.Add(IconPanel, 0, 0);
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

            InitCustomization();
            ResumeLayout();
        }

        protected void EditClick()
        {
            var mainForm = FindForm() as MainForm;

            if (mainForm == null) return;
            var chain = GenerateNoteChain();

            var creationController = new NoteCreationMenuController(mainForm);
            creationController.AddLinkedInfo(chain);

            var creationResult = creationController.OpenCreationMenu();
            creationResult?.CreateNote();
        }
        protected virtual LinkedList<SimpleNoteModel> GenerateNoteChain()
        {
            var chain = new LinkedList<SimpleNoteModel>();

            if (this is INoteControlChild asChild)
            {
                if (asChild.NoteType.GetInputTypeEquivalence() != null) chain.AddFirst((SimpleNoteModel)(NoteControl)asChild);
                for (var curr = (asChild as IControlParent) ?? asChild.ParentNote; curr != null && curr is NoteControl asCtrl; curr = (curr as INoteControlChild)?.ParentNote)
                    if (asCtrl.NoteType.GetInputTypeEquivalence() != null)
                        chain.AddFirst((SimpleNoteModel)asCtrl);
            }
            return chain;
        }

        public static explicit operator SimpleNoteModel(NoteControl from) => new SimpleNoteModel(from); 

        public override string ToString()
        {
            return $"{NoteName}";
        }
    }
}
