using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using Newtonsoft.Json;
using System.Collections;

namespace MusicLoverHandbook.Models.JSON
{
    public class InnerNotesConverter : JsonConverter
    {
        private JsonSerializerSettings usedSettings;

        public InnerNotesConverter(JsonSerializerSettings usedSettings)
        {
            this.usedSettings = usedSettings;
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableTo(typeof(IEnumerable))
                && objectType.GetGenericArguments().FirstOrDefault()?.IsAssignableTo(typeof(INote))
                    == true;
        }

        public override object? ReadJson(
            JsonReader reader,
            Type objectType,
            object? existingValue,
            JsonSerializer serializer
        )
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            IEnumerable<INote> inners = (IEnumerable<INote>)value!;
            inners = inners.Where(x => x.NoteType.IsInformaionCarrier());
            writer.WriteStartArray();
            foreach (var note in inners)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(note.NoteType.ToString(true));
                var jObj = JsonConvert.SerializeObject(note, usedSettings);
                writer.WriteRawValue(jObj);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}
