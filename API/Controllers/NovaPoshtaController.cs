using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Core.Entities.NovaPoshta;
using Core.Interfaces;

namespace API.Controllers
{
    public class NovaPoshtaController : BaseApiController
    {
        private readonly INovaPoshtaService _npService;

        public NovaPoshtaController(INovaPoshtaService npService)
        {
            _npService = npService;
        }
        
        [HttpGet("searchSettlements/{settlement}")]
        public async Task<ActionResult<List<SearchSettlementsResponse.Address>>> GetSettlements(string settlement)
        {
            return await _npService.SearchSettlements(settlement);            
        }

        [HttpGet("warehouses/{cityRef}")]
        public async Task<ActionResult<List<GetWarehousesResponse.DataArray>>> GetWarehouses(string cityRef)
        {
            return await _npService.GetWarehouses(cityRef);
        }
    }
}
