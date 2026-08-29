using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
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
        public async Task<ActionResult<BasketSummaryDto>> GetAggregateBasketSummary(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            if (basket == null)
            {
                return Ok(new BasketSummaryDto
                {
                    BasketId = id,
                    TotalItems = 0,
                    TotalQuantity = 0,
                    UniqueProducts = 0,
                    SubTotal = 0m,
                    AveragePrice = 0m,
                    TotalPrice = 0m,
                    EstimatedShipping = 0m,
                    ProductBreakdown = new List<BasketSummaryItemDto>()
                });
            }

            var items = basket.Items ?? new List<BasketItem>();
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var totalQuantity = items.Sum(item => item.Quantity);
            var uniqueProducts = items
                .Select(item => item.ProductName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();
            var averagePrice = totalQuantity > 0 ? subtotal / totalQuantity : 0m;
            var estimatedShipping = EstimateShipping(subtotal, totalQuantity);

            var productBreakdown = items
                .GroupBy(item => new
                {
                    Category = ResolveCategory(item),
                    Type = ResolveType(item),
                    Brand = ResolveBrand(item),
                    ProductName = item.ProductName ?? "Unknown product"
                })
                .Select(group =>
                {
                    var groupSubtotal = group.Sum(item => item.Price * item.Quantity);
                    var groupQuantity = group.Sum(item => item.Quantity);
                    var groupEstimatedShipping = EstimateShipping(groupSubtotal, groupQuantity);

                    return new BasketSummaryItemDto
                    {
                        Category = group.Key.Category,
                        Type = group.Key.Type,
                        ProductName = group.Key.ProductName,
                        Brand = group.Key.Brand,
                        Quantity = group.First().Quantity,
                        TotalQuantity = groupQuantity,
                        UniqueProducts = group
                            .Select(item => item.ProductName)
                            .Where(name => !string.IsNullOrWhiteSpace(name))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .Count(),
                        SubTotal = groupSubtotal,
                        AveragePrice = groupQuantity > 0 ? groupSubtotal / groupQuantity : 0m,
                        EstimatedShipping = groupEstimatedShipping,
                        Total = groupSubtotal + groupEstimatedShipping
                    };
                })
                .ToList();

            return Ok(new BasketSummaryDto
            {
                BasketId = basket.Id,
                TotalItems = items.Count,
                TotalQuantity = totalQuantity,
                UniqueProducts = uniqueProducts,
                SubTotal = subtotal,
                AveragePrice = averagePrice,
                TotalPrice = subtotal + estimatedShipping,
                EstimatedShipping = estimatedShipping,
                ProductBreakdown = productBreakdown
            });
        }

        [NonAction]
        public Task<ActionResult<BasketSummaryDto>> AggregateBasketSummary(string id)
        {
            return GetAggregateBasketSummary(id);
        }

        private static decimal EstimateShipping(decimal subtotal, int totalQuantity)
        {
            if (subtotal <= 0m)
            {
                return 0m;
            }

            return subtotal * 0.10m + totalQuantity * 0.50m;
        }

        private static string ResolveCategory(BasketItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.Type))
            {
                return item.Type;
            }

            return "Uncategorized";
        }

        private static string ResolveType(BasketItem item)
        {
            return string.IsNullOrWhiteSpace(item.Type) ? "Standard" : item.Type;
        }

        private static string ResolveBrand(BasketItem item)
        {
            if (string.IsNullOrWhiteSpace(item.ProductName))
            {
                return "Unknown";
            }

            var split = item.ProductName.Split('-', '/', '|');
            if (split.Length > 1 && !string.IsNullOrWhiteSpace(split[0]))
            {
                return split[0].Trim();
            }

            return item.Type ?? "Unknown";
        }
    }
}
