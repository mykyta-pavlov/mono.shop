namespace Core.Entities.NovaPoshta
{
    public class SearchSettlementsResponse
    {
        public DataArray[] Data { get; set; }

        public class DataArray
        {
            public Address[] Addresses { get; set; }
        }

        public class Address
        {
            public string Present { get; set; }
            public string DeliveryCity { get; set; }
        }
    }
}