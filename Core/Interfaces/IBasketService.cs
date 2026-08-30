using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBasketService
    {
        Task<BasketSummary> GetAggregateBasketSummaryAsync(string basketId);
    }
}
