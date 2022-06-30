using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MusicLoverHandbook.Models.JSON
{
    public class NoteDesrializationConverter : JsonConverter<NoteImportModel>
    {
        #region Public Properties

        public override bool CanWrite => false;

        #endregion Public Properties

        #region Public Methods

        public override NoteImportModel? ReadJson(
            JsonReader reader,
            Type objectType,
            NoteImportModel? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var obj = JObject.Load(reader);
            var repObj = GetCuttedTree(obj);
            var noteImport = JsonConvert.DeserializeObject<NoteImportModel>(
                repObj.ToString(),
                new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error }
            );

            return noteImport;
        }

        public override void WriteJson(
            JsonWriter writer,
            NoteImportModel? value,
            JsonSerializer serializer
        )
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Private Methods

        private JObject? CutProp(JToken obj) => obj.First!.Values<JObject>().First();

        private JObject GetCuttedTree(JObject obj)
        {
            var token = CutProp(obj)!;
            if (token.ContainsKey("InnerNotes"))
            {
                JArray jarr = new();
                foreach (var inner in token.GetValue("InnerNotes")!)
                    jarr.Add(GetCuttedTree(inner.ToObject<JObject>()!));
                token.GetValue("InnerNotes")!.Replace(jarr);
            }
            else
                token.Add("InnerNotes", null);

            return token;
        }

        #endregion Private Methods
    }
}
