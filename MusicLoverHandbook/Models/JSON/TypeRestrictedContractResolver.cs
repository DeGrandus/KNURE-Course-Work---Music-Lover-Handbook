using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MusicLoverHandbook.Models.JSON
{
    public class TypeRestrictedContractResolver : DefaultContractResolver
    {
        private string[] onlyNames;
        private Type[] usedTypes;

        public TypeRestrictedContractResolver(params Type[] specials)
        {
            usedTypes = specials;
            onlyNames = specials.SelectMany(x => x.GetProperties().Select(x => x.Name)).ToArray();
        }

        public static TypeRestrictedContractResolver operator &(
            TypeRestrictedContractResolver r1,
            TypeRestrictedContractResolver r2
        ) => new TypeRestrictedContractResolver(r1.usedTypes.Intersect(r2.usedTypes).ToArray());

        public static TypeRestrictedContractResolver operator |(
            TypeRestrictedContractResolver r1,
            TypeRestrictedContractResolver r2
        ) => new TypeRestrictedContractResolver(r1.usedTypes.Concat(r2.usedTypes).ToArray());

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
