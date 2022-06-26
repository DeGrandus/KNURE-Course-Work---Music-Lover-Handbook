using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using static MusicLoverHandbook.Controls_and_Forms.UserControls.InputData;

namespace MusicLoverHandbook.Logic.Notes
{
    public class NoteBuilder
    {
        public NoteBuilder(MainForm form)
        {
            Form = form;
        }

        public MainForm Form { get; }

        public NoteControlMidder CreateNote(
            IEnumerable<OutputInfo> infoOrdered,
            NoteCreationOrder creationOrder
        )
        {
            List<INoteControl> contaierData = Form.NotesContainer.InnerNotes
                .Cast<INoteControl>()
                .ToList();

            var order = creationOrder.GetOrder();

            NoteControlMidder? hierBase = null;
            NoteControlParent? parent = null;
            for (var currentNode = order.First; currentNode != null; currentNode = currentNode.Next)
            {
                var currentType = currentNode.Value;
                var currentInfo = infoOrdered.ToList().Find(x => x.Type == currentType);

                if (currentInfo == null || !currentInfo.IsValid())
                    continue;

                var currentNote =
                    contaierData.Find(
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
                            parent as IParentControl ?? Form.NotesContainer,
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
