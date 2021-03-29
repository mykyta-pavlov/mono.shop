using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities.NovaPoshta;
using Core.Interfaces;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class NovaPoshtaService : INovaPoshtaService
    {
        private readonly IConfiguration _config;

        public NovaPoshtaService(IConfiguration config)
        {
            _config = config;
        }
        
        public async Task<List<SearchSettlementsResponse.Address>> SearchSettlements(string settlement)
        {
            List<SearchSettlementsResponse.Address> settlements = new List<SearchSettlementsResponse.Address>();
            
            const string url = "http://api.novaposhta.ua/v2.0/json/Address/searchSettlements/";

            var npRequest = new SearchSettlementsRequest(
                _config["NovaPoshtaKey"], 
                "Address", 
                "searchSettlements", 
                settlement, 
                100);
            
            var json = JsonSerializer.Serialize(npRequest);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            
            ApiHelper.InitializeClient();

            using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(url, data))
            {
                if (response.IsSuccessStatusCode)
                {
                    SearchSettlementsResponse npResponse = await response.Content.ReadAsAsync<SearchSettlementsResponse>();
                    
                    foreach (var res in npResponse.Data)
                    {
                        foreach (var address in res.Addresses)
                        {
                            settlements.Add(address);
                        }
                    }
                    
                    return settlements;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<GetWarehousesResponse.DataArray>> GetWarehouses(string cityRef)
        {
            List<GetWarehousesResponse.DataArray> warehouses = new List<GetWarehousesResponse.DataArray>();
            
            const string url = "http://api.novaposhta.ua/v2.0/json/AddressGeneral/getWarehouses/";

            var npRequest = new GetWarehousesRequest(
                "AddressGeneral",
                "getWarehouses",
                _config["NovaPoshtaKey"],
                cityRef
            );
            
            var json = JsonSerializer.Serialize(npRequest);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            
            ApiHelper.InitializeClient();
            
            using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(url, data))
            {
                if (response.IsSuccessStatusCode)
                {
                    GetWarehousesResponse npResponse = await response.Content.ReadAsAsync<GetWarehousesResponse>();

                    foreach (var res in npResponse.Data)
                    {
                        warehouses.Add(res);
                    }
                    
                    return warehouses;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}