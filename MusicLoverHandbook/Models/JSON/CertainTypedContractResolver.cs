using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MusicLoverHandbook.Models.JSON
{
    public class CertainTypedContractResolver : DefaultContractResolver
    {
        private string[] onlyNames;
        private Type[] usedTypes;

        public CertainTypedContractResolver(params Type[] specials)
        {
            usedTypes = specials;
            onlyNames = specials.SelectMany(x => x.GetProperties().Select(x => x.Name)).ToArray();
        }

        public static CertainTypedContractResolver operator &(
            CertainTypedContractResolver r1,
            CertainTypedContractResolver r2
        ) => new CertainTypedContractResolver(r1.usedTypes.Intersect(r2.usedTypes).ToArray());

        public static CertainTypedContractResolver operator |(
            CertainTypedContractResolver r1,
            CertainTypedContractResolver r2
        ) => new CertainTypedContractResolver(r1.usedTypes.Concat(r2.usedTypes).ToArray());

        protected override IList<JsonProperty> CreateProperties(
            Type type,
            MemberSerialization memberSerialization
        )
        {
            var all = base.CreateProperties(type, memberSerialization);
            all = all.Where(x => onlyNames.Contains(x.PropertyName)).ToList();
            return all;
        }
    }
}
