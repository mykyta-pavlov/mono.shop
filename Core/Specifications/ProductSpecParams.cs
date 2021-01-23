namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex {get; set; } = 1;
        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public int? TypeId { get; set; }
        public bool? SizeAvailableXxs { get; set; }
        public bool? SizeAvailableXs { get; set; }
        public bool? SizeAvailableS { get; set; }
        public bool? SizeAvailableM { get; set; }
        public bool? SizeAvailableL { get; set; }
        public bool? SizeAvailableXl { get; set; }
        public bool? SizeAvailableXxl { get; set; }
        public string Sort {get; set; }
        private string _search;
        public string Search 
        { 
            get => _search;
            set => _search = value.ToLower();
        }
    }
}