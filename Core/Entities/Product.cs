namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public int SizeQuantityXxs { get; set; }
        public int SizeQuantityXs { get; set; }
        public int SizeQuantityS { get; set; }
        public int SizeQuantityM { get; set; }
        public int SizeQuantityL { get; set; }
        public int SizeQuantityXl { get; set; }
        public int SizeQuantityXxl { get; set; }
    }
}