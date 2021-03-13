namespace Core.Entities
{
    public class ProductSizeQuantity : BaseEntity
    {
        public string SizeName { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}