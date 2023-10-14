using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
 
namespace DevShop.Core.Helpers;

public class IgnoreJsonPropertyName : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var list = base.CreateProperties(type, memberSerialization);
        foreach (var prop in list)
        {
            prop.PropertyName = prop.UnderlyingName;
        }

        return list;
    }
}