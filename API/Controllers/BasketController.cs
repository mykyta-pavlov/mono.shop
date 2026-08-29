using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly Core.Interfaces.IProductRepository _productRepository;

        public BasketController(IBasketRepository basketRepository, IMapper mapper, Core.Interfaces.IProductRepository productRepository)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var customerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);

            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }

        [HttpGet("summary")]
        public async Task<ActionResult<BasketSummaryDto>> AggregateBasketSummary(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id) ?? new CustomerBasket(id);

            var items = basket.Items ?? new System.Collections.Generic.List<BasketItem>();

            // prefetch product details for distinct product ids
            var distinctIds = System.Linq.Enumerable.Distinct(items.Select(i => i.Id)).ToList();
            var productMap = new System.Collections.Generic.Dictionary<int, Core.Entities.Product>();
            foreach (var pid in distinctIds)
            {
                var prod = await _productRepository.GetProductByIdAsync(pid);
                productMap[pid] = prod;
            }

            // group by category/type/brand
            var groups = items.GroupBy(i =>
            {
                productMap.TryGetValue(i.Id, out var prod);
                var category = prod?.ProductType?.ProductCategory?.Name ?? "Unknown";
                var type = prod?.ProductType?.Name ?? i.Type ?? "Unknown";
                string brand = null; // no brand on Product entity
                return new { Category = category, Type = type, Brand = brand };
            });

            var breakdown = new System.Collections.Generic.List<BasketSummaryItemDto>();

            foreach (var g in groups)
            {
                var subTotal = g.Sum(i => i.Price * i.Quantity);
                var totalQuantity = g.Sum(i => i.Quantity);
                var uniqueProducts = g.Select(i => i.Id).Distinct().Count();
                var averagePrice = totalQuantity > 0 ? subTotal / totalQuantity : 0m; // per-item average
                var estimatedShipping = subTotal * 0.10m; // 10% estimated shipping (option 1)
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
                    SubTotal = subTotal,
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

            return Ok(result);
        }
    }
}
