using Fortnox.SDK.Serialization;
using Newtonsoft.Json;

namespace Fortnox.SDK.Entities;

[Entity(SingularName = "Expense", PluralName = "Expenses")]
public class Expense
{

    ///<summary> Unique expense code </summary>
    [JsonProperty]
    public string Code { get; set; }

    ///<summary> Description of expense </summary>
    [JsonProperty]
    public string Text { get; set; }

    ///<summary> Account number </summary>
    [JsonProperty]
    public long? Account { get; set; }
}