using System.Threading.Tasks;
using Fortnox.SDK.Connectors.Base;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Interfaces;
using Fortnox.SDK.Search;

namespace Fortnox.SDK.Connectors;

internal class SupplierInvoicePaymentConnector : SearchableEntityConnector<SupplierInvoicePayment, SupplierInvoicePaymentSubset, SupplierInvoicePaymentSearch>, ISupplierInvoicePaymentConnector
{
    public SupplierInvoicePaymentConnector()
    {
        Endpoint = Endpoints.SupplierInvoicePayments;
    }

    public async Task<EntityCollection<SupplierInvoicePaymentSubset>> FindAsync(SupplierInvoicePaymentSearch searchSettings)
    {
        return await BaseFind(searchSettings).ConfigureAwait(false);
    }

    public async Task DeleteAsync(long? id)
    {
        await BaseDelete(id.ToString()).ConfigureAwait(false);
    }

    public async Task<SupplierInvoicePayment> CreateAsync(SupplierInvoicePayment supplierInvoicePayment)
    {
        return await BaseCreate(supplierInvoicePayment).ConfigureAwait(false);
    }

    public async Task<SupplierInvoicePayment> UpdateAsync(SupplierInvoicePayment supplierInvoicePayment)
    {
        return await BaseUpdate(supplierInvoicePayment, supplierInvoicePayment.Number.ToString()).ConfigureAwait(false);
    }

    public async Task<SupplierInvoicePayment> GetAsync(long? id)
    {
        return await BaseGet(id.ToString()).ConfigureAwait(false);
    }

    public async Task BookkeepAsync(long? id)
    {
        await DoActionAsync(id.ToString(), Action.Bookkeep).ConfigureAwait(false);
    }
}