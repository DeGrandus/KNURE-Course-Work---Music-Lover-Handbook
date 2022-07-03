using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Controls_and_Forms.UserControls;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.NoteAlter;
using MusicLoverHandbook.View.Forms;
using System.Diagnostics;

namespace MusicLoverHandbook.Logic
{
    public class NoteCreationMenuController
    {
        #region Private Fields

        private NoteCreationMenu creationMenu;
        private MainForm mainForm;

        #endregion Private Fields

        #region Public Constructors

        public NoteCreationMenuController(MainForm mainForm)
        {
            creationMenu = new NoteCreationMenu(mainForm);
            this.mainForm = mainForm;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AppendInformation(SimpleNoteModel simpleModel)
        {
            if (simpleModel.NoteType is NoteType inputType)
            {
                CreationParamsControl? data = creationMenu.CreationParemControlsOrdered
                    .ToList()
                    .Find(x => x.InputType == inputType);
                if (data == null)
                    return;
                data.InputNameBox.Text = simpleModel.Name;
                data.InputDescriptionBox.Text = simpleModel.Description;
            }
        }

        public void AppendLinkedInformation(LinkedList<SimpleNoteModel> simpleModels)
        {
            if (
                simpleModels.First?.Value.NoteType.GetAssociatedCreationOrder()
                is NoteCreationOrder creationType
            )
                creationMenu.CreationOrder = creationType;
            foreach (var sModel in simpleModels)
                AppendInformation(sModel);
        }

        public NoteCreationResult? OpenCreationMenu()
        {
            if (creationMenu.ShowDialog() == DialogResult.OK)
            {
                return new NoteCreationResult(mainForm, creationMenu.FinalNote);
            }
            return null;
        }

        #endregion Public Methods

        #region Public Classes

        public class NoteCreationResult
        {
            #region Private Fields

            private MainForm mainForm;

            #endregion Private Fields

            #region Public Properties

            public NoteControlMidder? ResultNote { get; }

            #endregion Public Properties

            #region Public Constructors

            public NoteCreationResult(MainForm mainForm, NoteControlMidder? resultNote)
            {
                ResultNote = resultNote;
                this.mainForm = mainForm;
            }

            #endregion Public Constructors

            #region Public Methods

            public void CreateNote()
            {
                if (ResultNote != null)
                {
                    var wasOpened = ResultNote.IsOpened;
                    if (wasOpened)
                        ResultNote.OnDoubleClick();

                    if (mainForm.NotesContainer.InnerNotes.Contains(ResultNote))
                        mainForm.NotesContainer.Insert_AddNoteButton(ResultNote);
                    else
                    {
                        mainForm.NotesContainer.InnerNotes.Add(ResultNote);
                    }

                    if (wasOpened)
                        ResultNote.OnDoubleClick();
                }
            }

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}
