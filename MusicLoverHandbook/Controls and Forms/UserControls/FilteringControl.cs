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
    public partial class FilteringControl : UserControl
    {
        private NoteAdvancedFilterMenu filterMenu;
        public NoteType FilterNoteType { get; set; }
        private NoteLite[] lites;
        public FilteringControl(NoteAdvancedFilterMenu filterMenu, NoteLite[] oneTypedLites)
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
            noteNameLabel.Font = new(Font.FontFamily, 50, GraphicsUnit.Pixel);
            noteNameLabel.BackColor = filterMenu.titleLabel.BackColor;

            var hierTypes = lites.Where(x => x.Ref.UsedCreationOrder != null).SelectMany(x => x.Ref.UsedCreationOrder!.Value.GetOrder()).Distinct();

            foreach (var type in hierTypes)
            {
                options.Add(new(Color.Gray, ControlPaint.Light(type.GetLiteColor() ?? type.GetColor() ?? Color.LightGreen, -0.4f), false)
                {
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = Font,
                    Size = new(10, 100),
                    BasicTooltipText = "No affect",
                    SpecialTooltipText = $"All occurances of \"{type.ToString(true)}\" will be found in all prefiltered \"{FilterNoteType.ToString(true)}\" notes",
                    Tag = type,
                    SwitchType=BasicSwitchLabel.SwitchMode.Click
                });
            }
            ButtonsLinker();

            optionsFlow.Controls.AddRange(options.ToArray());
        }
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
