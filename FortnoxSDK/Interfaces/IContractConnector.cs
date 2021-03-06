using System.Threading.Tasks;
using Fortnox.SDK.Entities;
using Fortnox.SDK.Search;

// ReSharper disable UnusedMember.Global
namespace Fortnox.SDK.Interfaces
{
    /// <remarks/>
    public interface IContractConnector : IEntityConnector
	{

		Contract Update(Contract contract);
		Contract Create(Contract contract);
		Contract Get(long? id);
        EntityCollection<ContractSubset> Find(ContractSearch searchSettings);
		Contract Finish(long? id);
		Contract CreateInvoice(long? id);
		Contract IncreaseInvoiceCount(long? id);

		Task<Contract> UpdateAsync(Contract contract);
		Task<Contract> CreateAsync(Contract contract);
		Task<Contract> GetAsync(long? id);
        Task<EntityCollection<ContractSubset>> FindAsync(ContractSearch searchSettings);
        Task<Contract> FinishAsync(long? id);
        Task<Contract> CreateInvoiceAsync(long? id);
        Task<Contract> IncreaseInvoiceCountAsync(long? id);
	}
}
