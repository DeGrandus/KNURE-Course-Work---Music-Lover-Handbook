using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Controls_and_Forms.UserControls;
using MusicLoverHandbook.Models;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.View.Forms;

namespace MusicLoverHandbook.Logic
{
    public class NoteCreationMenuController
    {
        private MainForm mainForm;
        private NoteCreationMenu menu;

        public NoteCreationMenuController(MainForm mainForm)
        {
            menu = new NoteCreationMenu(mainForm);
            this.mainForm = mainForm;
        }

        public void AddInfo(SimpleNoteModel model)
        {
            if (model.NoteType is NoteType inputType)
            {
                CreationParamsControl? data = menu.InputDataOrdered
                    .ToList()
                    .Find(x => x.InputType == inputType);
                if (data == null)
                    return;
                data.InputNameBox.Text = model.Name;
                data.InputDescriptionBox.Text = model.Description;
            }
        }

        public void AddLinkedInfo(LinkedList<SimpleNoteModel> models)
        {
            if (
                models.First?.Value.NoteType.GetConnectedCreationOrder()
                is NoteCreationOrder creationType
            )
                menu.CreationOrder = creationType;
            foreach (var model in models)
                AddInfo(model);
        }

        public NoteCreationResult? OpenCreationMenu()
        {
            if (menu.ShowDialog() == DialogResult.OK)
            {
                return new NoteCreationResult(mainForm, menu.FinalNote);
            }
            return null;
        }

        public class NoteCreationResult
        {
            private MainForm mainForm;

            public NoteControlMidder? Result { get; }

            public NoteCreationResult(MainForm mainForm, NoteControlMidder? result)
            {
                Result = result;
                this.mainForm = mainForm;
            }

            public void CreateNote()
            {
                if (Result != null)
                {
                    var wasOpened = Result.IsOpened;
                    if (wasOpened)
                        Result.OnDoubleClick();

                    if (mainForm.NotesContainer.InnerNotes.Contains(Result))
                        mainForm.NotesContainer.SetupAddNoteButton(Result);
                    else
                    {
                        mainForm.NotesContainer.InnerNotes.Add(Result);
                    }

                    if (wasOpened)
                        Result.OnDoubleClick();
                }
            }
        }
    }
}