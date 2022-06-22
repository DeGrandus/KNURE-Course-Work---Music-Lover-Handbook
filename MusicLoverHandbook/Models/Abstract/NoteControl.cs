using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using System.Reflection;
using static MusicLoverHandbook.Models.Inerfaces.IControlTheme;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControl : UserControl, INoteControl, ICloneable
    {
        private ToolTip ballonTip;
        private bool isDeleteShown;
        private bool isEditShown;
        private bool isInfoShown;
        private TableLayoutPanel mainTable;
        private string noteDescription;
        private string noteText;
        private Color theme;

        public virtual object Clone()
        {
            var constructor = GetConstructorWithinRequested();
            var requested = ConstructorRequested;
            List<object?> parameters = new();
            foreach (var param in constructor.GetParameters())
            {
                var type = param.ParameterType;
                //MessageBox.Show($"{type}\n\n {string.Join("\n",requested.Select(x=>x.Type))}");
                var group = requested.First(p => p.Type == type || p.Type.IsSubclassOf(type) || type.GetInterface(p.Type.Name)!=null);
                parameters.Add(group.Data);
                requested.Remove(group);
            }
            
            return constructor.Invoke(parameters.ToArray());
        }
        protected virtual List<(Type Type, object? Data)> ConstructorRequested
        {
            get => new() { (typeof(string), NoteName), (typeof(string), NoteDescription), (typeof(NoteType), NoteType), (typeof(NoteCreationOrder?), UsedCreationOrder) };
        }
        private ConstructorInfo GetConstructorWithinRequested()
        {

            var constructors = GetType().GetConstructors();
            var requested = ConstructorRequested;
            var reqTypes = requested.Select(kv => kv.Type);
            var t = 1;
            return constructors.First(c => c.GetParameters().Select(p => p.ParameterType).All(pt => 
            {
                var s = reqTypes.Any(rt => rt == pt || rt.IsSubclassOf(pt) || pt.GetInterface(rt.Name) != null);
                //MessageBox.Show($"{t++}\n{pt}\n\n{string.Join(" \n ",reqTypes)}\n\n{s}");
                return s;
            }));
        }
        protected NoteControl(string text, string description, NoteType noteType, NoteCreationOrder? order)
        {
            BackColor = Color.Transparent;
            UsedCreationOrder = order;
            NoteType = noteType;
            SetupColorTheme(NoteType);
            InitCustomLayout();
            InitValues(text, description);
        }

        public event ThemeChangeEventHandler? ColorChanged;

        ControlCollection INoteControl.Controls => Controls;
        public ButtonPanel DeleteButton { get; private set; }
        public ButtonPanel EditButton { get; private set; }

        public Image? Icon
        {
            get => IconPanel.BackgroundImage;
            set
            {
                IconPanel.BackgroundImageLayout = ImageLayout.Stretch;
                IconPanel.BackgroundImage = value;
            }
        }

        public Panel IconPanel { get; private set; }
        public ButtonPanel InfoButton { get; private set; }

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

        public string NoteDescription
        {
            get => noteDescription;
            set
            {
                if (InfoButton != null)
                    ballonTip?.SetToolTip(InfoButton, value);
                noteDescription = value;
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

        public NoteType NoteType { get; }
        public SideButtonsPanel SideButtons { get; private set; }
        public Label TextLabel { get; private set; }

        public Color ThemeColor
        {
            get => theme;
            set
            {
                theme = value;
                OnColorChanged();
            }
        }

        public NoteCreationOrder? UsedCreationOrder { get; }
        protected virtual int sizeS { get; private set; } = 70;
        protected virtual float textSizeRatio { get; private set; } = 0.5f;

        public static explicit operator SimpleNoteModel(NoteControl from) =>
            new SimpleNoteModel(from);

        public virtual void ChangeSize(int size)
        {
            sizeS = size;
            InitCustomLayout();
        }

        public void OnColorChanged()
        {
            if (ColorChanged != null)
                ColorChanged(this, new(ThemeColor));
        }

        public virtual void SetupColorTheme(NoteType type)
        {
            ThemeColor = type.GetColor() ?? Color.Transparent;
        }

        public override string ToString()
        {
            return $"{NoteName}";
        }

        protected void EditClick()
        {
            var mainForm = FindForm() as MainForm;

            if (mainForm == null)
                return;
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
                if (asChild.NoteType.AsInputType() != null)
                    chain.AddFirst((SimpleNoteModel)(NoteControl)asChild);
                for (
                    var curr = (asChild as IControlParent) ?? asChild.ParentNote;
                    curr != null && curr is NoteControl asCtrl;
                    curr = (curr as INoteControlChild)?.ParentNote
                )
                    if (asCtrl.NoteType.AsInputType() != null)
                        chain.AddFirst((SimpleNoteModel)asCtrl);
            }
            return chain;
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
                    var box = MessageBox.Show(
                        $"Are you sure you want to delete {NoteType} {NoteName}?",
                        "Delete warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (box == DialogResult.Yes)
                        if (asChild.ParentNote is INoteControlParent asParent)
                            asParent.Linker.InnerNotes.Remove(asChild);
                        else
                            asChild.ParentNote.InnerNotes.Remove(asChild);
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

        protected virtual void InitValues(string text, string description)
        {
            NoteName = text;
            NoteDescription = description;
        }

        private void InitCustomization()
        {
            foreach (
                var control in new Control[] { TextLabel, InfoButton, DeleteButton, EditButton }
            )
            {
                control.MouseEnter += OnButtonMouseEnter;
                control.MouseLeave += OnButtonMouseLeave;
            }
        }

        private void OnButtonMouseEnter(object? sender, EventArgs e)
        {
            if (sender is Control control)
                control.BackColor = ControlPaint.Light(control.BackColor, 0.4f);
        }

        private void OnButtonMouseLeave(object? sender, EventArgs e)
        {
            if (sender is Control control)
                control.BackColor = ThemeColor;
        }

        private void ToogleViewing(bool isVis, Control toToggle)
        {
            if (!isVis)
                toToggle.Hide();
            else
                toToggle.Show();
        }
    }
}