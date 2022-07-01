using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.NoteAlter;
using System.Data;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls
{
    public partial class SmartFilteringOptionMenu : UserControl
    {
        #region Public Fields

        public NoteType? CurrentlySelectedTypeOption;
        public LiteNote[] OneTypeNotes;
        public BasicSwitchLabel SSLNSwitch;

        #endregion Public Fields

        #region Private Fields

        private AdvancedFilteringModeChangedHandler? advancedFilteringModeChange;
        private NoteAdvancedFilterMenu filterMenu;
        private bool isValid = true;
        private List<BasicSwitchLabel> options = new List<BasicSwitchLabel>();

        #endregion Private Fields

        #region Public Properties

        public NoteType FilterNoteType { get; set; }

        public bool IsValid
        {
            get => isValid;
            set
            {
                if (!value)
                    BackColor = Color.FromArgb(50, Color.Red);
                else
                    BackColor = filterMenu.BackColor;
                isValid = value;
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public SmartFilteringOptionMenu(NoteAdvancedFilterMenu filterMenu, LiteNote[] oneTypedLites)
        {
            InitializeComponent();

            var types = oneTypedLites.Select(x => x.OriginalNoteRefference.NoteType).Distinct();
            if (types.Count() != 1)
                throw new ArgumentException(
                    "Unable to handle more than one type of notes in FilteringControl constructor"
                );
            FilterNoteType = types.First();
            OneTypeNotes = oneTypedLites;
            this.filterMenu = filterMenu;

            SetupLayout();
        }

        #endregion Public Constructors

        #region Private Methods

        private void ButtonsLinker()
        {
            options.ForEach(x => x.SpecialStateChanged += OnFilteringModeChange);
        }

        private void OnFilteringModeChange(object? self, bool isSpecial)
        {
            if (isSpecial)
            {
                CurrentlySelectedTypeOption = ((BasicSwitchLabel)self!).Tag is NoteType type
                    ? type
                    : null;
                options
                    .Where(x => x != (BasicSwitchLabel)self!)
                    .ToList()
                    .ForEach(x => x.SpecialState = false);
            }
            else if (options.Where(x => x.SpecialState == true).Count() == 0)
            {
                CurrentlySelectedTypeOption = null;
            }
        }

        private void SetupLayout()
        {
            Font = filterMenu.Font;
            BackColor = filterMenu.BackColor;
            noteNameLabel.Text = FilterNoteType.ToString(true);
            noteNameLabel.BackColor = filterMenu.titleLabel.BackColor;

            noteNameLabel.Click += (sender, e) =>
            {
                IsValid = !IsValid;
            };

            var doSelect = true;
            var hierTypes = OneTypeNotes
                .Where(x => x.OriginalNoteRefference.UsedCreationOrder != null)
                .SelectMany(x => x.OriginalNoteRefference.UsedCreationOrder!.Value.GetOrder())
                .Distinct()
                .Reverse();
            foreach (var type in hierTypes)
            {
                var button = new BasicSwitchLabel(
                    Color.Gray,
                    ControlPaint.Light(
                        type.GetLiteColor() ?? type.GetColor() ?? Color.LightGreen,
                        -0.5f
                    ),
                    false
                )
                {
                    Text = type.ToString(true),
                    Dock = DockStyle.Top,
                    Font = new(Font.FontFamily, 12),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new(2, 50),
                    SwitchType = SwitchMode.Click,
                    BasicTooltipText = "No affect",
                    SpecialTooltipText =
                        $@"All occurances of ""{type.ToString(true)}"" will be selected in ""{FilterNoteType.ToString(true)}""",
                    Tag = type,
                };
                if (type == FilterNoteType)
                {
                    button.Enabled = false;
                    button.BasicBackColor = Color.LightGray;
                    button.ForeColor = Color.Gray;
                }
                else if (doSelect)
                {
                    button.initialState = true;
                    doSelect = false;
                }

                options.Add(button);
            }
            ButtonsLinker();

            optionsPanel.Controls.AddRange(options.ToArray());

            SSLNSwitch = new(Color.OrangeRed, Color.LightGreen, true)
            {
                Text = "S.S.L.N",
                BasicTooltipText = "Save Same-Level Notes (NO)",
                SpecialTooltipText = "Save Same-Level Notes (YES)",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            mainTable.Controls.Add(SSLNSwitch, 1, 2);
        }

        #endregion Private Methods

        #region Public Delegates

        public delegate void AdvancedFilteringModeChangedHandler(
            BasicSwitchLabel self,
            bool isSpecial
        );

        #endregion Public Delegates

        #region Public Events

        public event AdvancedFilteringModeChangedHandler AdvancedFilteringModeChange
        {
            add => advancedFilteringModeChange += value;
            remove => advancedFilteringModeChange -= value;
        }

        #endregion Public Events
    }
}
