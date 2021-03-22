namespace Core.Entities
{
    public class ImageUrls : BaseEntity
    {
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}