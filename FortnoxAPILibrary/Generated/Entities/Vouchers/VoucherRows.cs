using FortnoxAPILibrary.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FortnoxAPILibrary.Entities
{
    [Entity(SingularName = "VoucherRows", PluralName = "VoucherRows")]
    public class VoucherRows
    {

        ///<summary> Account number. The number must be of an existing active account. </summary>
        [JsonProperty]
        public int? Account { get; set; }

        ///<summary> Code of the cost center. The code must be of an existing cost center. </summary>
        [JsonProperty]
        public string CostCenter { get; set; }

        ///<summary> Amount of credit. </summary>
        [JsonProperty]
        public double? Credit { get; set; }

        ///<summary> The description of the account. </summary>
        [ReadOnly]
        [JsonProperty]
        public string Description { get; private set; }

        ///<summary> Amount of debit. </summary>
        [JsonProperty]
        public double? Debit { get; set; }

        ///<summary> Code of the project. The code must be of an existing project. </summary>
        [JsonProperty]
        public string Project { get; set; }

        ///<summary> If the row is marked as removed. </summary>
        [ReadOnly]
        [JsonProperty]
        public bool? Removed { get; private set; }

        ///<summary> Transaction information regarding the row. </summary>
        [JsonProperty]
        public string TransactionInformation { get; set; }
    }
}