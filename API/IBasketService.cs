using System.Threading.Tasks;
using API.Dtos;

namespace API.Services
{
    public interface IBasketService
    {
        Task<BasketSummaryDto> GetAggregateBasketSummary(string basketId);
    }
}
