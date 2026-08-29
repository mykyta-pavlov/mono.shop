using System.Collections.Generic;

namespace API.Dtos;

public class BasketSummaryDto
{
    public string BasketId { get; set; }
    public int TotalItems { get; set; }
    public int TotalQuantity { get; set; }
    public int UniqueProducts { get; set; }
    public decimal SubTotal { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal EstimatedShipping { get; set; }
    public List<BasketSummaryItemDto> ProductBreakdown { get; set; }
}