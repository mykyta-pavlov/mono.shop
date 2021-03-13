namespace Core.Entities
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
    }
}