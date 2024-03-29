﻿using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;

namespace MusicLoverHandbook.Models.JSON
{
    public class NoteImportModel : INote
    {
        #region Public Properties

        public (Type Type, object? Data)[] ConstructorData =>
            new (Type, object?)[]
            {
                (typeof(string), NoteName),
                (typeof(string), NoteDescription),
                (typeof(NoteType), NoteType),
                (typeof(NoteCreationOrder), UsedCreationOrder),
            };

        public List<NoteImportModel>? InnerNotes { get; set; }

        public string NoteDescription { get; set; }

        public string NoteName { get; set; }

        public NoteType NoteType { get; }

        public NoteCreationOrder? UsedCreationOrder { get; }

        #endregion Public Properties

        #region Public Constructors + Destructors

        public NoteImportModel(
            string noteName,
            string noteDescription,
            NoteType noteType,
            NoteCreationOrder usedCreationOrder,
            NoteImportModel[]? innerNotes
        )
        {
            NoteName = noteName;
            NoteDescription = noteDescription;
            NoteType = noteType;
            UsedCreationOrder = usedCreationOrder;
            InnerNotes = innerNotes?.ToList();
        }

        #endregion Public Constructors + Destructors

        #region Public Methods

        public override string ToString()
        {
            return $"{NoteType}/{NoteName}:\n-{string.Join("\n-", InnerNotes ?? new())}";
        }

        #endregion Public Methods
    }
}
