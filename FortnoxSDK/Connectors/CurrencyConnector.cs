using System.Threading.Tasks;
using Fortnox.SDK.Connectors.Base;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Interfaces;
using Fortnox.SDK.Search;

namespace Fortnox.SDK.Connectors;

internal class CurrencyConnector : SearchableEntityConnector<Currency, Currency, CurrencySearch>, ICurrencyConnector
{
    public CurrencyConnector()
    {
        Endpoint = Endpoints.Currencies;
    }

    public async Task<EntityCollection<Currency>> FindAsync(CurrencySearch searchSettings)
    {
        return await BaseFind(searchSettings).ConfigureAwait(false);
    }

    public async Task DeleteAsync(string id)
    {
        await BaseDelete(id).ConfigureAwait(false);
    }

    public async Task<Currency> CreateAsync(Currency currency)
    {
        return await BaseCreate(currency).ConfigureAwait(false);
    }

    public async Task<Currency> UpdateAsync(Currency currency)
    {
        return await BaseUpdate(currency, currency.Code).ConfigureAwait(false);
    }

    public async Task<Currency> GetAsync(string id)
    {
        return await BaseGet(id).ConfigureAwait(false);
    }
}