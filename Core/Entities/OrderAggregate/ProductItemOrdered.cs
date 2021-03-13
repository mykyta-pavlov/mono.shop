namespace Core.Entities.OrderAggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered() {}

        public ProductItemOrdered(int productItemId, string productName, string thumbnailUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            ThumbnailUrl = thumbnailUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
