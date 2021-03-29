namespace Core.Entities.NovaPoshta
{
    public class GetWarehousesResponse
    {
        public DataArray[] Data { get; set; }
        
        public class DataArray
        {
            public string SiteKey { get; set; }
            public string Description { get; set; }
            public string Ref { get; set; }
            public string SettlementRef { get; set; }
        }
    }
}