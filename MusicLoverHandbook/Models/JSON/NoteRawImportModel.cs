using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models.JSON
{
    public class NoteRawImportModel : INote
    {
        public (Type Type, object? Data)[] ConstructorData =>
            new (Type, object?)[]
            {
                (typeof(string), NoteName),
                (typeof(string), NoteDescription),
                (typeof(NoteType), NoteType),
                (typeof(NoteCreationOrder), UsedCreationOrder),
            };

        public List<NoteRawImportModel>? InnerNotes { get; set; }

        public string NoteDescription { get; set; }

        public string NoteName { get; set; }

        public NoteType NoteType { get; }

        public NoteCreationOrder? UsedCreationOrder { get; }

        public NoteRawImportModel(
            string noteName,
            string noteDescription,
            NoteType noteType,
            NoteCreationOrder usedCreationOrder,
            NoteRawImportModel[]? innerNotes
        )
        {
            NoteName = noteName;
            NoteDescription = noteDescription;
            NoteType = noteType;
            UsedCreationOrder = usedCreationOrder;
            InnerNotes = innerNotes?.ToList();
        }

        public override string ToString()
        {
            return $"{NoteType}/{NoteName}:\n-{string.Join("\n-", InnerNotes ?? new())}";
        }
    }
}
