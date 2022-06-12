using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Abstract;
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
    public partial class InputData : UserControl
    {
        private bool canNameBeEmpty;

        public InputType InputType { get; private set; }
        public SmartComboBox InputNameBox {get;}
        public TextBox InputDescriptionBox { get; }
        public InputData()
        {
            InitializeComponent();
            InputDescriptionBox = descriptionBox;
            InputNameBox = boxName;
            InputNameBox.StateChanged += (sender, state) =>
            {
                renameSection.Enabled = state == InputState.OK;
                if (!renameSection.Enabled)
                    renameCheck.Checked = false;
                InputDescriptionBox.Enabled = state != InputState.UNKNOWN && !state.IsError();
                UpdateRenameSection();
                if (state == InputState.OK)
                    InputDescriptionBox.Text = InputNameBox.InnerData.Find(x => x.NoteText == InputNameBox.Text)?.NoteDescription??"";

            };
            renameCheck.CheckedChanged += (sender, e) =>
            {
                UpdateRenameSection();
            };
        }
        public void UpdateRenameSection()
        {
            renameInput.Enabled = renameSection.Enabled && renameCheck.Checked;
        }
        public InputData(InputType mainType) : this()
        {
            InputType = mainType;
            SetLabel(InputType.ToString());
            
        }
        public void SetDataSource(NoteControlParent notes)
        {
            boxName.SetSource(notes);
        }
        public void SetDataSource<OnlyAllow>(NoteControlParent notes)
        {
            boxName.SetSource<OnlyAllow>(notes);
        }
        public void SetDataSource(NotesContainer container)
        {
            boxName.SetSource(container);
        }
        public void SetDataSource<OnlyAllow>(NotesContainer container)
        {
            boxName.SetSource<OnlyAllow>(container);

        }
        public void ClearDataSource()
        {
            boxName.ClearDataSource();
        }

        public void SetInputType(InputType type)
        {
            InputType = type;
            boxName.InputType = type;
            SetLabel(InputType.ToString());
        }
        public void SetLabel(string text)
        {
            noteTypeLabel.Text = text;
        }
        public void SetFont(Font font)
        {
            Font = font;
        }
        public void SetLabelFont(Font font)
        {
            noteTypeLabel.Font = font;
        }

        public class OutputData
        {
            public OutputData(InputType type, string text, string description)
            {
                Type = type;
                Text = text;
                Description = description;
            }

            public InputType Type { get; }
            public string Text { get; }
            public string Description { get; }

        }
    }

}
