namespace API.Dtos;

public class BasketSummaryItemDto
{
    public string Category { get; set; }
    public string Type { get; set; }
    public string ProductName { get; set; }
    public string Brand { get; set; }
    public int Quantity { get; set; }
    public int TotalQuantity { get; set; }
    public int UniqueProducts { get; set; }
    public decimal SubTotal { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal EstimatedShipping { get; set; }
    public decimal Total { get; set; }
}