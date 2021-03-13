using System.Collections.Generic;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<ImageUrls> ImageUrls { get; set; }
        public List<ProductSizeQuantity> ProductSizeQuantity { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
    }
}