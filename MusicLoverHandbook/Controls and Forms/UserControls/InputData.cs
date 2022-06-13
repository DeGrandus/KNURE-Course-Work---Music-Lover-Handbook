﻿using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using Newtonsoft.Json.Linq;
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
        private bool canNameBeEmpty = false;
        private bool isRenameInvalid = false;

        private bool IsRenameFieldTextInvalid
        {
            get => isRenameInvalid; set
            {
                isRenameInvalid = value;
            }
        }

        public InputType InputType { get; private set; }
        public SmartComboBox InputNameBox { get; }
        public TextBox InputDescriptionBox { get; }
        public bool AutoFill { get; set; } = true;


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
                if (!AutoFill) return;
                if (state == InputState.OK)
                    InputDescriptionBox.Text =
                        InputNameBox.InnerData
                            .Find(x => x.NoteText == InputNameBox.Text)
                            ?.NoteDescription ?? "";
            };
            renameCheck.CheckedChanged += (sender, e) =>
            {
                UpdateRenameSection();
            };
            renameInput.TextChanged += (sender, e) =>
            {
                UpdateRenameField();
            };
        }

        private void UpdateRenameField()
        {
            if (!renameInput.Enabled) { renameInput.BackColor = Color.White; return; }
            IsRenameFieldTextInvalid = boxName.Items.Cast<string>().Contains(renameInput.Text) || renameInput.Text.Length < 2;
            renameInput.BackColor = Color.FromArgb(255, Color.FromArgb((int)(!IsRenameFieldTextInvalid? InputState.OK : InputState.EMPTY_FIELD)));
        }
        public void UpdateRenameSection()
        {
            renameInput.Enabled = renameSection.Enabled && renameCheck.Checked;
            UpdateRenameField();
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
        public OutputInfo GetOutput()
        {
            if (renameInput.Enabled && IsRenameFieldTextInvalid)
                if (renameInput.Text.Length == 0)
                    throw new InvalidDataException($"{InputType} cannot be renamed to an empty field.");
                else if (renameInput.Text.Length == 1)
                    throw new InvalidDataException($"{InputType} cannot be renamed to \"{renameInput.Text}\". It is yoo short.");

                else
                    throw new InvalidDataException($"{InputType} cannot be renamed to \"{renameInput.Text}\". An {InputType.ToString().ToLower()} with this name is already exists in current category.");
            return new OutputInfo(InputType,
                InputNameBox.Status == InputState.OK ||
                InputNameBox.Status == InputState.CREATION ?
                InputNameBox.Text : null, InputDescriptionBox.Text,
                renameInput.Enabled ? renameInput.Text.Length > 1 ? renameInput.Text : null : null,
                Enabled);
        }
        public void Clean()
        {
            InputNameBox.Text = "";
            InputDescriptionBox.Text = "";
        }

        public class OutputInfo
        {
            public OutputInfo(InputType type, string? text, string description, string? replacementText, bool enabled)
            {
                Enabled = enabled;
                Type = type;
                Text = text;
                Description = description;
                ReplacementText = replacementText;
            }
            public bool Enabled { get; }
            public InputType Type { get; }
            public string? Text { get; }
            public string Description { get; }
            public string? ReplacementText { get; }
            public bool IsValid() => Enabled && Text != null;
        }
    }
}
