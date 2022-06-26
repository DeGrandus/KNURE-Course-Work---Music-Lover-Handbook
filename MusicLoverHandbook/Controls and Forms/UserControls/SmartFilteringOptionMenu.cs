using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicLoverHandbook.Controls_and_Forms.UserControls
{
    public partial class SmartFilteringOptionMenu : UserControl
    {
        private NoteAdvancedFilterMenu filterMenu;
        public NoteType FilterNoteType { get; set; }
        private NoteLite[] lites;
        public SmartFilteringOptionMenu(NoteAdvancedFilterMenu filterMenu, NoteLite[] oneTypedLites)
        {
            InitializeComponent();

            var types = oneTypedLites.Select(x => x.Ref.NoteType).Distinct();
            if (types.Count() != 1)
                throw new ArgumentException("Unable to handle more than one type of notes in FilteringControl constructor");
            FilterNoteType = types.First();
            lites = oneTypedLites;
            this.filterMenu = filterMenu;

            SetupLayout();
        }
        private List<BasicSwitchLabel> options = new List<BasicSwitchLabel>();
        private void SetupLayout()
        {
            Font = filterMenu.Font;
            BackColor = filterMenu.BackColor;
            noteNameLabel.Text = FilterNoteType.ToString(true);
            noteNameLabel.BackColor = filterMenu.titleLabel.BackColor;

            var doSelect = true;
            var hierTypes = lites.Where(x => x.Ref.UsedCreationOrder != null).SelectMany(x => x.Ref.UsedCreationOrder!.Value.GetOrder()).Distinct().Reverse();
            foreach (var type in hierTypes)
            {
                var button = new BasicSwitchLabel(Color.Gray, ControlPaint.Light(type.GetLiteColor() ?? type.GetColor() ?? Color.LightGreen, -0.5f), false)
                {
                    Text = type.ToString(true),
                    Dock = DockStyle.Top,
                    Font = new(Font.FontFamily, 12),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new(2, 50),
                    SwitchType = BasicSwitchLabel.SwitchMode.Click,
                    BasicTooltipText = "No affect",
                    SpecialTooltipText = $@"All occurances of ""{type.ToString(true)}"" will be selected in ""{FilterNoteType.ToString(true)}""",
                    Tag = type,
                };
                if (type == FilterNoteType)
                {
                    button.Enabled = false;
                    button.BasicBackColor = Color.LightGray;
                    button.ForeColor = Color.Gray;
                } else if (doSelect)
                {
                    button.InitialState = true;
                    doSelect = false;
                }

                options.Add(button);
            }
            ButtonsLinker();

            optionsPanel.Controls.AddRange(options.ToArray());

            sslnSwitch = new(Color.OrangeRed, Color.LightGreen, true)
            {
                Text = "S.S.L.N",
                BasicTooltipText = "Save Same-Level Notes (NO)",
                SpecialTooltipText = "Save Same-Level Notes (YES)",
                Dock=DockStyle.Fill,
                TextAlign=ContentAlignment.MiddleCenter
            };
            mainTable.Controls.Add(sslnSwitch,1,2);
        }
        private BasicSwitchLabel sslnSwitch;
        private void ButtonsLinker()
        {
            options.ForEach(x=>x.SpecialStateChanged+=OnFilteringModeChange);
        }
        private void OnFilteringModeChange(object? self, bool isSpecial)
        {
            if (isSpecial)
                options.Where(x => x != (BasicSwitchLabel)self!).ToList().ForEach(x=>x.SpecialState = false);
        }
        public delegate void AdvancedFilteringModeChangeEventHandler(BasicSwitchLabel self,bool isSpecial);
        private AdvancedFilteringModeChangeEventHandler? advancedFilteringModeChange;
        public event AdvancedFilteringModeChangeEventHandler AdvancedFilteringModeChange
        {
            add => advancedFilteringModeChange += value;
            remove => advancedFilteringModeChange -= value;
        }
    }
}
