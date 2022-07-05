using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Models.JSON;
using MusicLoverHandbook.Models.Managers;
using MusicLoverHandbook.Models.NoteAlter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MusicLoverHandbook.Models.Abstract
{
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControl : UserControl, INoteControl
    {
        #region Private Fields

        private ToolTip ballonTip;
        private bool isDeleteShown;
        private bool isEditShown;
        private bool isInfoShown;
        private bool isMarked;
        private TableLayoutPanel mainTable;

        private Panel mark =
            new()
            {
                BackColor = Color.FromArgb(150, 255, 150),
                Dock = DockStyle.Fill,
                Width = 14,
                Margin = new(0)
            };

        private string noteDescription;
        private string noteText;
        private Color theme;

        #endregion Private Fields

        #region Public Properties

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

        public bool IsMarked
        {
            get => isMarked;
            set
            {
                if (isMarked != value)
                {
                    isMarked = value;
                    RedrawMarked();
                }
            }
        }

        public Color MainColor
        {
            get => theme;
            set { theme = value; }
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
        public NoteCreationOrder? UsedCreationOrder { get; }

        #endregion Public Properties

        #region Protected Properties

        protected virtual int sizeS { get; private set; } = 70;

        protected virtual float textSizeRatio { get; private set; } = 0.5f;

        #endregion Protected Properties

        #region Protected Constructors + Destructors

        protected NoteControl(
            string text,
            string description,
            NoteType noteType,
            NoteCreationOrder? order
        )
        {
            BackColor = Color.Transparent;
            UsedCreationOrder = order;
            NoteType = noteType;
            SetupColorTheme(NoteType);
            SetupLayout();
            InitValues(text, description);
        }

        #endregion Protected Constructors + Destructors

        #region Public Methods

        public static explicit operator SimpleNoteModel(NoteControl fromNoteControl) =>
            new SimpleNoteModel(fromNoteControl);

        public virtual void ChangeSize(int size)
        {
            sizeS = size;
            SetupLayout();
        }

        public INoteControl Clone()
        {
            var impToClone = Deserialize();
            var manager = FindForm() is MainForm mf ? mf.RawNoteManager : new();
            var cloned = manager.RecreateFromImported(impToClone);
            return cloned;
        }

        public NoteImportModel Deserialize()
        {
            return JsonConvert.DeserializeObject<NoteImportModel>(
                Serialize(),
                FileManager.Instance.SerializerSettings
            )!;
        }

        public override bool Equals(object? obj)
        {
            return obj is NoteControl control
                && NoteDescription == control.NoteDescription
                && NoteName == control.NoteName
                && NoteType == control.NoteType;
        }

        public virtual List<LiteNote> Flatten()
        {
            return new() { new(NoteName, NoteDescription, this) };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NoteDescription, NoteName, NoteType);
        }

        public virtual void InvokeActionHierarcaly(Action<INoteControl> action)
        {
            action(this);
        }

        public bool RoughEquals(object? obj)
        {
            return Equals(obj)
                || obj is LiteNote lite
                    && NoteDescription == lite.NoteDescription
                    && NoteName == lite.NoteName
                    && NoteType == lite.NoteType;
        }

        public string Serialize()
        {
            var selfJson = JsonConvert.DeserializeObject<JToken>(
                JsonConvert.SerializeObject(this, FileManager.Instance.SerializerSettings)
            );
            var jObj = new JObject();
            jObj.Add(NoteType.ToString(true), selfJson);
            return jObj.ToString(Formatting.Indented);
        }

        public virtual void SetupColorTheme(NoteType type)
        {
            MainColor = type.GetColor() ?? Color.Transparent;
        }

        public LiteNote SingleFlatten()
        {
            return new(NoteName, NoteDescription, this);
        }

        public override string ToString()
        {
            return $@"{GetType().Name} : [ Name: {NoteName} | Desc: {NoteDescription} | Type: {NoteType} ]";
        }

        #endregion Public Methods

        #region Protected Methods

        protected void EditClick()
        {
            var mainForm = FindForm() as MainForm;

            if (mainForm == null)
                return;
            var chain = GenerateNoteChain();

            var creationController = new NoteCreationMenuController(mainForm);
            creationController.AppendLinkedInformation(chain);

            var creationResult = creationController.OpenCreationMenu();
            creationResult?.CreateNote();
        }

        protected virtual LinkedList<SimpleNoteModel> GenerateNoteChain()
        {
            var chain = new LinkedList<SimpleNoteModel>();

            if (this is INoteControlChild asChild)
            {
                if (asChild.NoteType.IsInformaionCarrier())
                    chain.AddFirst((SimpleNoteModel)(NoteControl)asChild);
                for (
                    var curr = (asChild as IParentControl) ?? asChild.ParentNote;
                    curr != null && curr is NoteControl asCtrl;
                    curr = (curr as INoteControlChild)?.ParentNote
                )
                    if (asCtrl.NoteType.IsInformaionCarrier())
                        chain.AddFirst((SimpleNoteModel)asCtrl);
            }
            return chain;
        }

        protected virtual void InitValues(string text, string description)
        {
            NoteName = text;
            NoteDescription = description;
        }

        protected virtual void SetupLayout()
        {
            SuspendLayout();
            Controls.Remove(mainTable);

            var font = FontManager.Instance.LoadedFamilies[0];
            Font = new Font(font, sizeS * textSizeRatio, GraphicsUnit.Pixel);

            Padding = new Padding(0);
            mainTable = new TableLayoutPanel()
            {
                Padding = new Padding(0),
                Margin = new Padding(0),
                Dock = DockStyle.Top,
                RowCount = 1,
                ColumnCount = 3,
            };
            SideButtons = new SideButtonsPanel(DockStyle.Right) { AutoSize = true, };
            IconPanel = new Panel()
            {
                Padding = new Padding(0),
                Margin = new Padding(0),
                BackColor = ControlPaint.Light(MainColor)
            };

            var textPanel = new Panel() { Padding = new Padding(0), Margin = new Padding(0), };

            Setup_MainElements();
            Setup_MainElements_Events();

            var comboPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0),
                Margin = new Padding(0),
            };

            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, sizeS));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, sizeS));

            textPanel.Dock = DockStyle.Fill;
            textPanel.Controls.Add(TextLabel);

            comboPanel.Controls.Add(textPanel);
            comboPanel.Controls.Add(SideButtons);

            mainTable.Controls.Add(IconPanel, 1, 0);
            mainTable.Controls.Add(comboPanel, 2, 0);

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

        #endregion Protected Methods

        #region Private Methods

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
                control.BackColor = MainColor;
        }

        private void RedrawMarked()
        {
            if (mainTable == null)
                return;
            if (IsMarked)
                mainTable.Controls.Add(mark, 0, 0);
            else
                mainTable.Controls.Remove(mark);
        }

        private void Setup_MainElements()
        {
            TextLabel = new Label()
            {
                UseMnemonic = false,
                Padding = new Padding(0),
                Margin = new Padding(0),
                Text = NoteName,
                BackColor = MainColor,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };

            InfoButton = new ButtonPanel(ButtonType.Info, 0)
            {
                BackColor = ControlPaint.Light(MainColor),
                BackgroundImageLayout = ImageLayout.Stretch,
                BackgroundImage = Properties.Resources.InfoIcon,
                Size = new Size(sizeS, sizeS)
            };

            Setup_ToolTip();
            ballonTip.SetToolTip(InfoButton, NoteDescription);
            DeleteButton = new ButtonPanel(ButtonType.Delete, 2)
            {
                BackColor = ControlPaint.Light(MainColor),
                BackgroundImageLayout = ImageLayout.Stretch,
                BackgroundImage = Properties.Resources.DeleteIcon,
                Size = new Size(sizeS, sizeS)
            };

            EditButton = new ButtonPanel(ButtonType.Edit, 1)
            {
                BackColor = ControlPaint.Light(MainColor),
                BackgroundImageLayout = ImageLayout.Stretch,
                BackgroundImage = Properties.Resources.EditIcon,
                Size = new Size(sizeS, sizeS),
            };
        }

        private void Setup_MainElements_Events()
        {
            EditButton.Click += (sender, e) => EditClick();
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
                    if (box == 
                    DialogResult.Yes)
                    {
                        if (asChild.ParentNote is IParentControl asParent)
                        {
                            asParent.InnerNotes.Remove(asChild);
                            var container =
                                (asParent as INoteControlChild)?.GetFirstParent() ?? asParent;
                            FileManager.Instance.HistoryManager.UpdateHistory(container);
                        }
                    }
                }
            };
        }

        private void Setup_ToolTip()
        {
            ballonTip = new ToolTip();
            ballonTip.IsBalloon = true;
            ballonTip.UseFading = true;
            ballonTip.UseAnimation = true;
            ballonTip.ToolTipIcon = ToolTipIcon.Info;
            ballonTip.ToolTipTitle = "Description";
            ballonTip.InitialDelay = 100;
        }

        private void ToogleViewing(bool isVis, Control toToggle)
        {
            if (!isVis)
                toToggle.Hide();
            else
                toToggle.Show();
        }

        #endregion Private Methods
    }
}
