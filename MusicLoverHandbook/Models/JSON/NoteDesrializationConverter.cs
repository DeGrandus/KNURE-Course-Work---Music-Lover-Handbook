using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MusicLoverHandbook.Models.JSON
{
    public class NoteDesrializationConverter : JsonConverter<NoteRawImportModel>
    {
        public override bool CanWrite => false;

        public override NoteRawImportModel? ReadJson(
            JsonReader reader,
            Type objectType,
            NoteRawImportModel? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var obj = JObject.Load(reader);
            var repObj = GetCuttedTree(obj);
            var noteImport = JsonConvert.DeserializeObject<NoteRawImportModel>(
                repObj.ToString(),
                new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error }
            );

            return noteImport;
        }

        public override void WriteJson(
            JsonWriter writer,
            NoteRawImportModel? value,
            JsonSerializer serializer
        )
        {
            throw new NotImplementedException();
        }

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
    }
}
