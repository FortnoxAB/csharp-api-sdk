using Fortnox.SDK.Serialization;
using Newtonsoft.Json;

namespace Fortnox.SDK.Entities;

[Entity(SingularName = "AssetsDepreciationListFields", PluralName = "AssetsDepreciationListFields")]
public class AssetsDepreciationListFields
{

    ///<summary> Direct URL to the record </summary>
    [ReadOnly]
    [JsonProperty]
    public string URL { get; private set; }

    ///<summary> Accounted value of an asset </summary>
    [ReadOnly]
    [JsonProperty]
    public long? AccountedValue { get; private set; }

    ///<summary> Depreciation amount of an asset </summary>
    [ReadOnly]
    [JsonProperty]
    public long? DepreciationAmount { get; private set; }

    ///<summary> Depreciation period of an asset </summary>
    [ReadOnly]
    [JsonProperty]
    public string DepreciationPeriod { get; private set; }
}