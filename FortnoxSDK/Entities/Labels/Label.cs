using Fortnox.SDK.Serialization;
using Newtonsoft.Json;

namespace Fortnox.SDK.Entities;

[Entity(SingularName = "Label", PluralName = "Labels")]
public class Label
{

    ///<summary> The ID of the label. </summary>
    [ReadOnly]
    [JsonProperty]
    public long? Id { get; private set; }

    ///<summary> Description of the label </summary>
    [JsonProperty]
    public string Description { get; set; }
}