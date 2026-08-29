using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;

namespace API.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;

        public BasketService(IBasketRepository basketRepository, IProductRepository productRepository)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
        }

        public async Task<BasketSummaryDto> GetAggregateBasketSummary(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId) ?? new CustomerBasket(basketId);

            var items = basket.Items ?? new List<BasketItem>();

            var distinctIds = items.Select(i => i.Id).Distinct().ToList();
            var productMap = new Dictionary<int, Product>();

            foreach (var pid in distinctIds)
            {
                var prod = await _productRepository.GetProductByIdAsync(pid);
                productMap[pid] = prod;
            }

            var groups = items.GroupBy(i =>
            {
                productMap.TryGetValue(i.Id, out var prod);
                var category = prod?.ProductType?.ProductCategory?.Name ?? "Unknown";
                var type = prod?.ProductType?.Name ?? i.Type ?? "Unknown";
                string brand = null;
                return new { Category = category, Type = type, Brand = brand };
            });

            var breakdown = new List<BasketSummaryItemDto>();

            foreach (var g in groups)
            {
                var subTotal = g.Sum(i => i.Price * i.Quantity);
                var totalQuantity = g.Sum(i => i.Quantity);
                var uniqueProducts = g.Select(i => i.Id).Distinct().Count();
                var averagePrice = totalQuantity > 0 ? subTotal / totalQuantity : 0m;
                var estimatedShipping = subTotal * 0.10m;
                var total = subTotal + estimatedShipping;

                var productNames = string.Join(", ", g.Select(i => i.ProductName).Distinct());

                breakdown.Add(new BasketSummaryItemDto
                {
                    Category = g.Key.Category,
                    Type = g.Key.Type,
                    Brand = g.Key.Brand,
                    ProductName = productNames,
                    Quantity = g.Sum(i => i.Quantity),
                    TotalQuantity = totalQuantity,
                    UniqueProducts = uniqueProducts,
                    SubTotal = decimal.Round(subTotal, 2),
                    AveragePrice = decimal.Round(averagePrice, 2),
                    EstimatedShipping = decimal.Round(estimatedShipping, 2),
                    Total = decimal.Round(total, 2)
                });
            }

            var overallSubTotal = breakdown.Sum(b => b.SubTotal);
            var overallEstimatedShipping = breakdown.Sum(b => b.EstimatedShipping);
            var overallTotalQuantity = items.Sum(i => i.Quantity);
            var overallUniqueProducts = items.Select(i => i.Id).Distinct().Count();
            var overallAveragePrice = overallTotalQuantity > 0 ? overallSubTotal / overallTotalQuantity : 0m;

            var result = new BasketSummaryDto
            {
                BasketId = basket.Id,
                TotalItems = items.Count,
                TotalQuantity = overallTotalQuantity,
                UniqueProducts = overallUniqueProducts,
                SubTotal = decimal.Round(overallSubTotal, 2),
                AveragePrice = decimal.Round(overallAveragePrice, 2),
                EstimatedShipping = decimal.Round(overallEstimatedShipping, 2),
                TotalPrice = decimal.Round(overallSubTotal + overallEstimatedShipping, 2),
                ProductBreakdown = breakdown
            };

            return result;
        }
    }
}
