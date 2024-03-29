using Fortnox.SDK.Serialization;
using Newtonsoft.Json;

namespace Fortnox.SDK.Entities;

[Entity(SingularName = "TrustedSender", PluralName = "TrustedSenders")]
public class TrustedEmailSender
{

    ///<summary> Id of the record </summary>
    [ReadOnly]
    [JsonProperty]
    public long? Id { get; private set; }

    ///<summary> Email address of trusted/rejected sender </summary>
    [JsonProperty]
    public string Email { get; set; }
}