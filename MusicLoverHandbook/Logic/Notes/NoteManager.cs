using MusicLoverHandbook.Controls_and_Forms.Forms;
using MusicLoverHandbook.Models.Abstract;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Models.JSON;

namespace MusicLoverHandbook.Logic.Notes
{
    public class NoteManager
    {
        private MainForm? mainForm;

        public NoteManager(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        public NoteControl RecreateFromImported(NoteRawImportModel model)
        {
            return RecreateFromImported(
                model,
                new (Type, object?)[] { (typeof(IControlParent), mainForm?.Container) }
            );
        }
        public NoteManager()
        {
            mainForm = null;
        }

        public NoteControl RecreateFromImported(NoteRawImportModel model, (Type, object?)[] adj)
        {
            var modelData = model.ConstructorData.ToList();
            modelData = modelData.Concat(adj).ToList();
            var modelConstructors = model.NoteType.GetConnectedNoteType()!.GetConstructors();
            var suitableConstructor = modelConstructors
                .ToList()
                .Find(
                    x =>
                        x.GetParameters()
                            .Select(x => x.ParameterType)
                            .All(
                                x =>
                                    modelData
                                        .Select(k => k.Type)
                                        .Any(k => k == x || k.IsAssignableTo(x))
                            )
                );
            if (suitableConstructor == null)
                throw new Exception(
                    $"Note recreation error: NoteType:[ {model.NoteType} ] do not have suitable constructor for given mathcing data"
                );

            List<object?> orderedConstructorParamObjects = new();
            foreach (var param in suitableConstructor.GetParameters())
            {
                var objset = modelData.Find(
                    x => x.Type == param.ParameterType || x.Type.IsAssignableTo(param.ParameterType)
                );
                modelData.Remove(objset);
                orderedConstructorParamObjects.Add(objset.Data);
            }

            NoteControl recreated = (NoteControl)suitableConstructor.Invoke(
                orderedConstructorParamObjects.ToArray()
            );
            if (recreated is INoteControlParent asParent)
                foreach (var innerModel in model.InnerNotes!)
                    asParent.InnerNotes.Add(
                        (INoteControlChild)RecreateFromImported(
                            innerModel,
                            new (Type, object?)[] { (asParent.GetType(), asParent) }
                        )
                    );

            return recreated;
        }
    }
}
