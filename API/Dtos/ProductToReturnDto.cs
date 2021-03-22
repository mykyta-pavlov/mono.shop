using System.Collections.Generic;
using Core.Entities;

namespace API.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<string> ImageUrls { get; set; }
        public Dictionary<string, bool> SizesAvailable { get; set; }
        public string ProductType { get; set; }
    }
}