namespace Core.Entities.NovaPoshta
{
    public class SearchSettlementsRequest
    {
        public SearchSettlementsRequest(string apiKey, string modelName, string calledMethod, string cityName, int limit)
        {
            this.apiKey = apiKey;
            this.modelName = modelName;
            this.calledMethod = calledMethod;
            methodProperties = new MethodProperties(cityName, limit);
        }

        public string apiKey { get; set; }
        public string modelName { get; set; }
        public string calledMethod { get; set; }
        public MethodProperties methodProperties { get; set; }
        
        public class MethodProperties
        {
            public MethodProperties(string cityName, int limit)
            {
                this.CityName = cityName;
                this.Limit = limit;
            }
            
            public string CityName { get; set; }
            public int Limit { get; set; }
        }
    }
}