using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly IBasketService _basketService;

        public BasketController(IBasketRepository basketRepository, IMapper mapper, IBasketService basketService)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _basketService = basketService;
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
            var summary = await _basketService.GetAggregateBasketSummaryAsync(id);

            var dto = new BasketSummaryDto
            {
                BasketId = summary.BasketId,
                TotalItems = summary.TotalItems,
                TotalQuantity = summary.TotalQuantity,
                UniqueProducts = summary.UniqueProducts,
                SubTotal = summary.SubTotal,
                AveragePrice = summary.AveragePrice,
                TotalPrice = summary.TotalPrice,
                EstimatedShipping = summary.EstimatedShipping,
                ProductBreakdown = summary.ProductBreakdown.Select(pb => new BasketSummaryItemDto
                {
                    Category = "Unknown",
                    Type = pb.Type,
                    Brand = "Unknown",
                    ProductName = pb.ProductName,
                    Quantity = pb.Quantity,
                    TotalQuantity = pb.Quantity,
                    UniqueProducts = 1,
                    SubTotal = pb.Price * pb.Quantity,
                    AveragePrice = pb.Price,
                    EstimatedShipping = summary.SubTotal > 0 ? Math.Round(summary.EstimatedShipping * (pb.Price * pb.Quantity / summary.SubTotal), 2) : 0,
                    Total = (pb.Price * pb.Quantity) + (summary.SubTotal > 0 ? Math.Round(summary.EstimatedShipping * (pb.Price * pb.Quantity / summary.SubTotal), 2) : 0)
                }).ToList()
            };

            return Ok(dto);
        }
    }
}
