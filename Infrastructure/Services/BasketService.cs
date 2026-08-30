using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;

        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<BasketSummary> GetAggregateBasketSummaryAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId) ?? new CustomerBasket(basketId);

            var totalItems = basket.Items.Count;
            var totalQuantity = basket.Items.Sum(i => i.Quantity);
            var uniqueProducts = basket.Items.Select(i => i.Id).Distinct().Count();
            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);
            var averagePrice = totalQuantity > 0 ? subTotal / totalQuantity : 0m;

            // Estimated shipping: base 5 + 1 per unique product
            var estimatedShipping = 5m + 1m * uniqueProducts;
            var totalPrice = subTotal + estimatedShipping;

            // Group products by Category/Type/Brand where available. BasketItem currently has Type, no Category/Brand.
            // Use Type as primary grouping key; set Category/Brand to "Unknown" when not available.
            var productBreakdown = basket.Items
                .GroupBy(i => i.Type ?? "Unknown")
                .Select(g => new BasketItem
                {
                    Id = 0,
                    ProductName = g.Key, // use group key as name
                    Price = g.Sum(i => i.Price * i.Quantity) / (g.Sum(i => i.Quantity) > 0 ? g.Sum(i => i.Quantity) : 1),
                    Quantity = g.Sum(i => i.Quantity),
                    PictureUrl = g.FirstOrDefault()?.PictureUrl,
                    Type = g.Key
                })
                .ToList();

            return new BasketSummary
            {
                BasketId = basket.Id,
                TotalItems = totalItems,
                TotalQuantity = totalQuantity,
                UniqueProducts = uniqueProducts,
                SubTotal = subTotal,
                AveragePrice = averagePrice,
                TotalPrice = totalPrice,
                EstimatedShipping = estimatedShipping,
                ProductBreakdown = productBreakdown
            };
        }
    }
}
