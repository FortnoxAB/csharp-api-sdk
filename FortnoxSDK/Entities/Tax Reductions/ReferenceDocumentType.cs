using System.Runtime.Serialization;

namespace Fortnox.SDK.Entities;

public enum ReferenceDocumentType
{
    [EnumMember(Value = "OFFER")]
    Offer,
    [EnumMember(Value = "ORDER")]
    Order,
    [EnumMember(Value = "INVOICE")]
    Invoice,
}