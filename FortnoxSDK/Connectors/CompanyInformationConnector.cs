using System.Threading.Tasks;
using Fortnox.SDK.Connectors.Base;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Interfaces;

namespace Fortnox.SDK.Connectors;

internal class CompanyInformationConnector : EntityConnector<CompanyInformation>, ICompanyInformationConnector
{
    public CompanyInformationConnector()
    {
        Endpoint = Endpoints.CompanyInformation;
    }

    public async Task<CompanyInformation> GetAsync()
    {
        return await BaseGet().ConfigureAwait(false);
    }
}