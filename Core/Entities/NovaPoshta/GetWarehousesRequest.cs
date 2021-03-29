namespace Core.Entities.NovaPoshta
{
    public class GetWarehousesRequest
    {
        public GetWarehousesRequest(string modelName, string calledMethod, string apiKey, string cityRef)
        {
            this.modelName = modelName;
            this.calledMethod = calledMethod;
            methodProperties = new MethodProps(apiKey, cityRef);
        }
        
        public string modelName { get; set; }
        public string calledMethod { get; set; }
        public MethodProps methodProperties { get; set; }

        public class MethodProps
        {
            public MethodProps(string apiKey, string cityRef)
            {
                this.apiKey = apiKey;
                this.CityRef = cityRef;
            }
            
            public string apiKey { get; set; }
            public string CityRef { get; set; }
        }

    }
}