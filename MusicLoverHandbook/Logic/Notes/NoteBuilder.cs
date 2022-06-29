using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Inerfaces;
using static MusicLoverHandbook.Controls_and_Forms.UserControls.CreationParamsControl;

namespace MusicLoverHandbook.Logic.Notes
{
    public class NoteBuilder
    {
        #region Public Properties

        public MainForm MainForm { get; }

        #endregion Public Properties



        #region Public Constructors

        public NoteBuilder(MainForm form)
        {
            MainForm = form;
        }

        #endregion Public Constructors



        #region Public Methods

        public NoteControlMidder CreateNote(
            IEnumerable<OutputInfo> infoOrdered,
            NoteCreationOrder creationOrder
        )
        {
            List<INoteControl> parentContainerData = MainForm.NotesContainer.InnerNotes
                .Cast<INoteControl>()
                .ToList();

            var creationOrderTypes = creationOrder.GetOrder();

            NoteControlMidder? hierarchyStarter = null;
            NoteControlParent? parent = null;
            for (
                var currentNode = creationOrderTypes.First;
                currentNode != null;
                currentNode = currentNode.Next
            )
            {
                var currentType = currentNode.Value;
                var currentInfo = infoOrdered.ToList().Find(x => x.Type == currentType);

                if (currentInfo == null || !currentInfo.IsValid())
                    continue;

                var currentNote =
                    parentContainerData.Find(
                        x =>
                            x.NoteName == currentInfo.Text
                            && x.GetType() == currentInfo.Type.GetConnectedNoteType()
                    ) as NoteControl;
                if (currentNote == null)
                {
                    currentNote = (NoteControl)currentType
                        .GetConnectedNoteType()!
                        .GetConstructors()[0].Invoke(
                        new object?[]
                        {
                            parent as IParentControl ?? MainForm.NotesContainer,
                            currentInfo.Text,
                            currentInfo.Description,
                            creationOrder
                        }
                    );
                    if (parent != null)
                        parent.InnerNotes.Add((INoteControlChild)currentNote);
                }
                else if (currentInfo.ReplacementText != null)
                    currentNote.NoteName = currentInfo.ReplacementText;
                currentNote.NoteDescription = currentInfo.Description;

                if (hierarchyStarter == null)
                    hierarchyStarter = (NoteControlMidder)currentNote;

                if (currentNote is not NoteControlParent)
                    break;
                parentContainerData = ((NoteControlParent)currentNote).InnerNotes
                    .Cast<INoteControl>()
                    .ToList();
                parent = (NoteControlParent)currentNote;
            }
            if (hierarchyStarter == null)
                throw new Exception("Somthing went wrong in creating Notes. Base note is null");
            return hierarchyStarter;
        }

        #endregion Public Methods
    }
}
