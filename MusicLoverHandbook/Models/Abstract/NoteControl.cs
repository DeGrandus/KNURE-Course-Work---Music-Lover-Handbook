using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Logic;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using static MusicLoverHandbook.Models.Inerfaces.IControlTheme;

namespace MusicLoverHandbook.Models.Abstract
{
    public class OnlySpecialTypesContractResolver : DefaultContractResolver
    {
        private string[] onlyNames;
        private Type[] usedTypes;
        
        public OnlySpecialTypesContractResolver(params Type[] specials)
        {
            usedTypes = specials;
            onlyNames = specials.SelectMany(x => x.GetProperties().Select(x => x.Name)).ToArray();
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var all = base.CreateProperties(type, memberSerialization);
            all = all.Where(x => onlyNames.Contains(x.PropertyName)).ToList();
            return all;
        }
        public static OnlySpecialTypesContractResolver operator |(OnlySpecialTypesContractResolver r1, OnlySpecialTypesContractResolver r2) => new OnlySpecialTypesContractResolver(r1.usedTypes.Concat(r2.usedTypes).ToArray());
        public static OnlySpecialTypesContractResolver operator &(OnlySpecialTypesContractResolver r1, OnlySpecialTypesContractResolver r2) => new OnlySpecialTypesContractResolver(r1.usedTypes.Intersect(r2.usedTypes).ToArray());
    }
    public class InnerNotesConverter : JsonConverter
    {
        private JsonSerializerSettings usedSettings;
        public override bool CanRead => false;
        public InnerNotesConverter(JsonSerializerSettings usedSettings)
        {
            this.usedSettings = usedSettings;
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableTo(typeof(IEnumerable)) && objectType.GetGenericArguments().FirstOrDefault()?.IsAssignableTo(typeof(INote)) == true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            IEnumerable<INote> inners = (IEnumerable<INote>)value!;
            inners = inners.Where(x => x.NoteType.IsInformaionCarrier());
            writer.WriteStartArray();
            foreach (var note in inners)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(note.NoteType.ToString(true));
                var jObj = JsonConvert.SerializeObject(note,usedSettings);
                writer.WriteRawValue(jObj);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

        }
    }
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NoteControl : UserControl, INoteControl
    {
        private ToolTip ballonTip;
        private bool isDeleteShown;
        private bool isEditShown;
        private bool isInfoShown;
        private TableLayoutPanel mainTable;
        private string noteDescription;
        private string noteText;
        private Color theme;
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
                if (asChild.NoteType.IsInformaionCarrier())
                    chain.AddFirst((SimpleNoteModel)(NoteControl)asChild);
                for (
                    var curr = (asChild as IControlParent) ?? asChild.ParentNote;
                    curr != null && curr is NoteControl asCtrl;
                    curr = (curr as INoteControlChild)?.ParentNote
                )
                    if (asCtrl.NoteType.IsInformaionCarrier())
                        chain.AddFirst((SimpleNoteModel)asCtrl);
            }
            return chain;
        }
        public string Serialize()
        {
            var selfJson = JsonConvert.DeserializeObject<JToken>(JsonConvert.SerializeObject(this, SerializerSettings));
            var jObj = new JObject();
            jObj.Add(NoteType.ToString(true), selfJson);
            return jObj.ToString(Formatting.Indented);
        }
        public NoteControl Deserialize()
        {
            throw new NotImplementedException();
        }
        private List<JsonConverter> GetConverters(JsonSerializerSettings settings)
        {
            return new() { new StringEnumConverter(), new InnerNotesConverter(settings) };
        }
        private JsonSerializerSettings SerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = ContractResolver,
                    Formatting = Formatting.Indented,
                };
                settings.Converters = GetConverters(settings);
                return settings;
            }
        }

        protected virtual OnlySpecialTypesContractResolver ContractResolver => new OnlySpecialTypesContractResolver(typeof(INote));
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
            TextLabel.MouseClick += (sender, e) =>
            {
                if (e.Button != MouseButtons.Right) return;

                MessageBox.Show(Serialize());
                //MessageBox.Show(Deseriali);
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