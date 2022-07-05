using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Extensions;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Models.JSON;

namespace MusicLoverHandbook.Logic.Notes
{
    public class NoteImportManager
    {
        #region Private Fields

        private MainForm? mainForm;

        #endregion Private Fields

        #region Public Constructors + Destructors

        public NoteImportManager(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        public NoteImportManager()
        {
            mainForm = null;
        }

        #endregion Public Constructors + Destructors

        #region Public Methods

        public NoteControl RecreateFromImported(NoteImportModel model)
        {
            return RecreateFromImported(
                model,
                new (Type, object?)[] { (typeof(IParentControl), mainForm?.Container) }
            );
        }

        public NoteControl RecreateFromImported(NoteImportModel model, (Type, object?)[] adj)
        {
            var modelData = model.ConstructorData.ToList();
            modelData = modelData.Concat(adj).ToList();
            var modelConstructors = model.NoteType.GetConnectedNoteType()!.GetConstructors();
            var suitableConstructor = modelConstructors
                .ToList()
                .Find(
                    constructor =>
                        constructor
                            .GetParameters()
                            .Select(parameter => parameter.ParameterType)
                            .All(
                                type =>
                                    modelData
                                        .Select(dataCluster => dataCluster.Type)
                                        .Any(
                                            clusterType =>
                                                clusterType == type
                                                || clusterType.IsAssignableTo(type)
                                        )
                            )
                );
            if (suitableConstructor == null)
                throw new Exception(
                    $"Note recreation error: NoteType:[ {model.NoteType} ] "
                        + $"do not have suitable constructor for given mathcing data"
                );

            List<object?> orderedConstructorParamObjects = new();
            foreach (var param in suitableConstructor.GetParameters())
            {
                var modelDataCluster = modelData.Find(
                    dataCluster =>
                        dataCluster.Type == param.ParameterType
                        || dataCluster.Type.IsAssignableTo(param.ParameterType)
                );
                modelData.Remove(modelDataCluster);
                orderedConstructorParamObjects.Add(modelDataCluster.Data);
            }

            NoteControl recreated = (NoteControl)suitableConstructor.Invoke(
                orderedConstructorParamObjects.ToArray()
            );
            if (recreated is INoteControlParent asParent)
                foreach (var innerRawNote in model.InnerNotes!)
                    asParent.InnerNotes.Add(
                        (INoteControlChild)RecreateFromImported(
                            innerRawNote,
                            new (Type, object?)[] { (asParent.GetType(), asParent) }
                        )
                    );

            return recreated;
        }

        #endregion Public Methods
    }
}
