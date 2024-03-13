using System;
using Fortnox.SDK.Serialization;
using Newtonsoft.Json;

// ReSharper disable UnusedMember.Global

namespace Fortnox.SDK.Entities;

/// <remarks/>
[Entity(SingularName = "Order", PluralName = "Orders")]
public class OrderSubset
{
    ///<summary> Direct url to the record and URL to Taxreduction for the order (URL to Taxreduction shows even if � Taxreduction are connected to order) </summary>
    [ReadOnly]
    [JsonProperty("@url")]
    public Uri Url { get; private set; }

    ///<summary> If Order is cancelled </summary>
    [ReadOnly]
    [JsonProperty]
    public bool? Cancelled { get; private set; }

    ///<summary> Currency </summary>
    [JsonProperty]
    public string Currency { get; set; }

    ///<summary> Customer name </summary>
    [JsonProperty]
    public string CustomerName { get; set; }

    ///<summary> Customer number </summary>
    [JsonProperty]
    public string CustomerNumber { get; set; }

    ///<summary> Delivery date </summary>
    [JsonProperty]
    public DateTime? DeliveryDate { get; set; }

    ///<summary> Document number </summary>
    [JsonProperty]
    public long? DocumentNumber { get; set; }

    ///<summary> External invoice reference 1 </summary>
    [JsonProperty]
    public string ExternalInvoiceReference1 { get; set; }

    ///<summary> External invoice reference 2 </summary>
    [JsonProperty]
    public string ExternalInvoiceReference2 { get; set; }

    ///<summary> Date of order </summary>
    [JsonProperty]
    public DateTime? OrderDate { get; set; }

    ///<summary> Project code </summary>
    [JsonProperty]
    public string Project { get; set; }

    ///<summary> Total amount </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? Total { get; private set; }
    
    ///<summary> Total amount to pay </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? TotalToPay { get; private set; }
}