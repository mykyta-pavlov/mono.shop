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
        public async Task<ActionResult<BasketSummaryDto>> AggregateBasketSummary(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            if (basket == null)
            {
                return Ok(new BasketSummaryDto
                {
                    BasketId = id,
                    ProductBreakdown = new List<BasketSummaryItemDto>()
                });
            }

            var groupedItems = basket.Items
                .GroupBy(item => new
                {
                    Category = item.Type ?? "Uncategorized",
                    Type = item.Type ?? "Unknown",
                    Brand = "Unknown"
                })
                .Select(group =>
                {
                    var subtotal = group.Sum(item => item.Price * item.Quantity);
                    var totalQuantity = group.Sum(item => item.Quantity);
                    var uniqueProducts = group
                        .Select(item => item.Id > 0 ? (object)item.Id : (object)item.ProductName)
                        .Distinct()
                        .Count();
                    var averagePrice = totalQuantity > 0 ? subtotal / totalQuantity : 0m;
                    var estimatedShipping = subtotal > 0 ? subtotal * 0.10m : 0m;

                    return new BasketSummaryItemDto
                    {
                        Category = group.Key.Category,
                        Type = group.Key.Type,
                        Brand = group.Key.Brand,
                        ProductName = string.Join(", ", group.Select(item => item.ProductName).Distinct()),
                        Quantity = group.Sum(item => item.Quantity),
                        TotalQuantity = totalQuantity,
                        UniqueProducts = uniqueProducts,
                        SubTotal = subtotal,
                        AveragePrice = averagePrice,
                        EstimatedShipping = estimatedShipping,
                        Total = subtotal + estimatedShipping,
                    };
                })
                .ToList();

            var totalQuantityAcrossBasket = basket.Items.Sum(item => item.Quantity);
            var subtotalAcrossBasket = basket.Items.Sum(item => item.Price * item.Quantity);
            var uniqueProductsAcrossBasket = basket.Items
                .Select(item => item.Id > 0 ? (object)item.Id : (object)item.ProductName)
                .Distinct()
                .Count();
            var estimatedShippingAcrossBasket = subtotalAcrossBasket > 0 ? subtotalAcrossBasket * 0.10m : 0m;

            var summary = new BasketSummaryDto
            {
                BasketId = basket.Id ?? id,
                TotalItems = totalQuantityAcrossBasket,
                TotalQuantity = totalQuantityAcrossBasket,
                UniqueProducts = uniqueProductsAcrossBasket,
                SubTotal = subtotalAcrossBasket,
                AveragePrice = totalQuantityAcrossBasket > 0 ? subtotalAcrossBasket / totalQuantityAcrossBasket : 0m,
                TotalPrice = subtotalAcrossBasket + estimatedShippingAcrossBasket,
                EstimatedShipping = estimatedShippingAcrossBasket,
                ProductBreakdown = groupedItems
            };

            return Ok(summary);
        }
    }
}
