namespace API.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductType { get; set; }
        public bool SizeAvailableXxs { get; set; }
        public bool SizeAvailableXs { get; set; }
        public bool SizeAvailableS { get; set; }
        public bool SizeAvailableM { get; set; }
        public bool SizeAvailableL { get; set; }
        public bool SizeAvailableXl { get; set; }
        public bool SizeAvailableXxl { get; set; }
    }
}