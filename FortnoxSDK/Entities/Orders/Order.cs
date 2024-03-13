using Fortnox.SDK.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fortnox.SDK.Entities;

[Entity(SingularName = "Order", PluralName = "Orders")]
public class Order
{

    ///<summary> Direct url to the record and URL to Taxreduction for the order (URL to Taxreduction shows even if – Taxreduction are connected to order) </summary>
    [ReadOnly]
    [JsonProperty("@url")]
    public Uri Url { get; private set; }

    ///<summary> Administration fee </summary>
    [JsonProperty]
    public decimal? AdministrationFee { get; set; }

    ///<summary> VAT of the administration fee </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? AdministrationFeeVAT { get; private set; }

    ///<summary> Address 1 </summary>
    [JsonProperty]
    public string Address1 { get; set; }

    ///<summary> Address 2 </summary>
    [JsonProperty]
    public string Address2 { get; set; }

    ///<summary> The amount that Taxreduction is based on </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? BasisTaxReduction { get; private set; }

    ///<summary> If Order is cancelled </summary>
    [ReadOnly]
    [JsonProperty]
    public bool? Cancelled { get; private set; }

    ///<summary> City </summary>
    [JsonProperty]
    public string City { get; set; }

    ///<summary> Comments </summary>
    [JsonProperty]
    public string Comments { get; set; }

    ///<summary> Contribution in Percent </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? ContributionPercent { get; private set; }

    ///<summary> Contribution in amount </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? ContributionValue { get; private set; }

    ///<summary> If remarks shall be copied from order to invoice </summary>
    [JsonProperty]
    public bool? CopyRemarks { get; set; }

    ///<summary> Country, Must be a name of an existing country </summary>
    [JsonProperty]
    public string Country { get; set; }

    ///<summary> Cost center </summary>
    [JsonProperty]
    public string CostCenter { get; set; }

    ///<summary> Currency </summary>
    [JsonProperty]
    public string Currency { get; set; }

    ///<summary> Currency rate </summary>
    [JsonProperty]
    public decimal? CurrencyRate { get; set; }

    ///<summary> Currency unit </summary>
    [JsonProperty]
    public decimal? CurrencyUnit { get; set; }

    ///<summary> Customer name </summary>
    [JsonProperty]
    public string CustomerName { get; set; }

    ///<summary> Customer number </summary>
    [JsonProperty]
    public string CustomerNumber { get; set; }

    ///<summary> Delivery address 1 </summary>
    [JsonProperty]
    public string DeliveryAddress1 { get; set; }

    ///<summary> Delivery address 2 </summary>
    [JsonProperty]
    public string DeliveryAddress2 { get; set; }

    ///<summary> Delivery City </summary>
    [JsonProperty]
    public string DeliveryCity { get; set; }

    ///<summary> Delivery Country </summary>
    [JsonProperty]
    public string DeliveryCountry { get; set; }

    ///<summary> Delivery date </summary>
    [JsonProperty]
    public DateTime? DeliveryDate { get; set; }

    ///<summary> Delivery name </summary>
    [JsonProperty]
    public string DeliveryName { get; set; }
    
    [WarehouseRequired]
    [JsonProperty]
    public DeliveryState? DeliveryState { get; set; }

    ///<summary> Delivery zip code </summary>
    [JsonProperty]
    public string DeliveryZipCode { get; set; }

    ///<summary> Document number </summary>
    [JsonProperty]
    public long? DocumentNumber { get; set; }

    ///<summary>  </summary>
    [JsonProperty]
    public EmailInformation EmailInformation { get; set; }

    ///<summary> External invoice reference 1 </summary>
    [JsonProperty]
    public string ExternalInvoiceReference1 { get; set; }

    ///<summary> External invoice reference 2 </summary>
    [JsonProperty]
    public string ExternalInvoiceReference2 { get; set; }

    ///<summary> Freight </summary>
    [JsonProperty]
    public decimal? Freight { get; set; }

    ///<summary> VAT of the freight </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? FreightVAT { get; private set; }

    ///<summary> Gross value of order </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? Gross { get; private set; }

    ///<summary> If Order is marked as housework </summary>
    [ReadOnly]
    [JsonProperty]
    public bool? HouseWork { get; private set; }

    ///<summary> Reference if an invoice is created from order </summary>
    [ReadOnly]
    [JsonProperty]
    public long? InvoiceReference { get; private set; }
    
    ///<summary> The properties for the object in this array is listed in the table “Labels” </summary>
    [JsonProperty]
    public IList<LabelReference> Labels { get; set; }
    
    ///<summary> Language code. Can be SV or EN. </summary>
    [JsonProperty]
    public Language? Language { get; set; }

    ///<summary> Net amount </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? Net { get; private set; }

    ///<summary> If order is set to complete </summary>
    [JsonProperty]
    public bool? NotCompleted { get; set; }

    ///<summary> Date of order </summary>
    [JsonProperty]
    public DateTime? OrderDate { get; set; }

    ///<summary> Reference to offer number </summary>
    [ReadOnly]
    [JsonProperty]
    public long? OfferReference { get; private set; }

    ///<summary>  </summary>
    [JsonProperty]
    public IList<OrderRow> OrderRows { get; set; }

    ///<summary> Type of the Order. Can be Order or Backorder. </summary>
    [ReadOnly]
    [JsonProperty]
    public OrderType? OrderType { get; private set; }

    ///<summary> Organization number </summary>
    [ReadOnly]
    [JsonProperty]
    public string OrganisationNumber { get; private set; }

    ///<summary> Our reference </summary>
    [JsonProperty]
    public string OurReference { get; set; }

    ///<summary> Phone 1 </summary>
    [JsonProperty]
    public string Phone1 { get; set; }

    ///<summary> Phone 2 </summary>
    [JsonProperty]
    public string Phone2 { get; set; }

    ///<summary> Price list code </summary>
    [JsonProperty]
    public string PriceList { get; set; }

    ///<summary> Print document template </summary>
    [JsonProperty]
    public string PrintTemplate { get; set; }

    ///<summary> Project code </summary>
    [JsonProperty]
    public string Project { get; set; }

    /// <summary> The date that the document was marked as ready in warehouse. </summary>
    [JsonProperty]
    public DateTime? OutboundDate { get; set; }

    ///<summary> Remarks on order </summary>
    [JsonProperty]
    public string Remarks { get; set; }

    ///<summary> Roundof amount </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? RoundOff { get; private set; }

    ///<summary> If document is printed or e-mailed </summary>
    [ReadOnly]
    [JsonProperty]
    public bool? Sent { get; private set; }
    
    /// <summary> The stock point that the items are to taken from or has been taken from. </summary>
    [JsonProperty]
    public string StockPointCode { get; set; }

    /// <summary> The stock point that the items are to taken from or has been taken from. </summary>
    [JsonProperty]
    public string StockPointId { get; set; }

    ///<summary> Amount of the Taxreduction </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? TaxReduction { get; private set; }

    /// <summary>  Tax Reduction Type </summary>
    [JsonProperty]
    public TaxReductionType? TaxReductionType { get; set; }

    ///<summary> Terms of delivery code </summary>
    [JsonProperty]
    public string TermsOfDelivery { get; set; }

    ///<summary> Terms of payment code </summary>
    [JsonProperty]
    public string TermsOfPayment { get; set; }

    ///<summary> Total amount </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? Total { get; private set; }
    
    ///<summary> Total amount to pay </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? TotalToPay { get; private set; }

    ///<summary> Total vat amount </summary>
    [ReadOnly]
    [JsonProperty]
    public decimal? TotalVAT { get; private set; }

    ///<summary> If order row price exclude or include vat </summary>
    [JsonProperty]
    public bool? VATIncluded { get; set; }

    ///<summary> Code of delivery </summary>
    [JsonProperty]
    public string WayOfDelivery { get; set; }
    
    /// <summary> Used to see if the document has been marked as ready in warehouse. </summary>
    [ReadOnly]
    [JsonProperty]
    public bool? WarehouseReady { get; private set; }

    ///<summary> Customer reference </summary>
    [JsonProperty]
    public string YourReference { get; set; }

    ///<summary> Customer order number </summary>
    [JsonProperty]
    public string YourOrderNumber { get; set; }

    ///<summary> Order zip code </summary>
    [JsonProperty]
    public string ZipCode { get; set; }
}