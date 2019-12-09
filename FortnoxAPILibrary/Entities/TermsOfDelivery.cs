using System;
using System.ComponentModel;
using Newtonsoft.Json;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace FortnoxAPILibrary.Entities
{
    /// <remarks/>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
	[Entity(SingularName = "TermsOfDelivery", PluralName = "TermsOfDeliveries")]
	public class TermsOfDelivery : TermsOfDeliverySubset
	{
		/// <remarks/>
		[JsonProperty]
		public string Code { get; set; }

        /// <remarks/>
        [JsonProperty]
        public string Description { get; set; }

        /// <summary>This field is Read-Only in Fortnox</summary>
		[JsonProperty(PropertyName = "@url")]
		public string Url { get; private set; }
    }

    /// <remarks/>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    [Entity(SingularName = "TermsOfDelivery", PluralName = "TermsOfDeliveries")]
    public class TermsOfDeliverySubset
    {
        /// <remarks/>
        public string Code { get; set; }

        /// <remarks/>
        public string Description { get; set; }

        /// <remarks/>
        [JsonProperty(PropertyName = "@url")]
        public string Url { get; private set; }
    }
}
