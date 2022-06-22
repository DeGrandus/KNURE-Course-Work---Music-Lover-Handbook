using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using static MusicLoverHandbook.Controls_and_Forms.UserControls.InputData;

namespace MusicLoverHandbook.Logic.Notes
{
    public class NoteBuilder
    {
        private NoteCreationOrder creationOrder;

        public NoteBuilder(
            MainForm form,
            LinkedList<OutputInfo> infoOrdered,
            NoteCreationOrder creationOrder
        )
        {
            OutputOrdered = infoOrdered;
            Form = form;
            this.creationOrder = creationOrder;
        }

        public MainForm Form { get; }
        public Dictionary<InputType, OutputInfo> Info { get; }
        public LinkedList<OutputInfo> OutputOrdered { get; }

        public NoteControlMidder CreateNote()
        {
            List<INoteControl> contaierData = Form.NotesContainer.InnerNotes
                .Cast<INoteControl>()
                .ToList();

            NoteControlMidder? hierBase = null;
            NoteControlParent? parent = null;
            for (
                var currentNode = OutputOrdered.First;
                currentNode != null;
                currentNode = currentNode.Next
            )
            {
                var currentInfo = currentNode.Value;
                var currentType = currentInfo.Type;

                if (!currentInfo.IsValid())
                    continue;

                var currentNote =
                    contaierData.Find(
                        x =>
                            x.NoteName == currentInfo.Text
                            && x.GetType() == currentInfo.Type.GetConnectedNoteType()
                    ) as NoteControl;
                if (currentNote == null)
                {
                    currentNote = (NoteControl)currentType.GetConnectedNoteType().GetConstructors()[
                        0
                    ].Invoke(
                        new object?[]
                        {
                            parent as IControlParent ?? Form.NotesContainer,
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

                if (hierBase == null)
                    hierBase = (NoteControlMidder)currentNote;

                if (currentNote is not NoteControlParent)
                    break;
                contaierData = ((NoteControlParent)currentNote).InnerNotes
                    .Cast<INoteControl>()
                    .ToList();
                parent = (NoteControlParent)currentNote;
            }
            if (hierBase == null)
                throw new Exception("Somthing went wrong in creating Notes. Base note is null");
            return hierBase;
        }
    }
}