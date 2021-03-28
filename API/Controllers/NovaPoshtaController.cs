using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        
    }
}