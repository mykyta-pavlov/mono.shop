using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.NovaPoshta;

namespace Core.Interfaces
{
    public interface INovaPoshtaService
    {
        Task<List<SearchSettlementsResponse.Address>> SearchSettlements(string settlementName);
    }
}