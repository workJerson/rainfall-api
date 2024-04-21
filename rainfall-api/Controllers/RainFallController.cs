using Microsoft.AspNetCore.Mvc;
using rainfall_api.Controllers.Common;
using rainfall_api.Dtos;
using rainfall_api.Services;

namespace rainfall_api.Controllers
{
    public class RainFallController : BaseApiController
    {
        [HttpGet("id/{stationId}/readings")]
        public async Task<RainfallReadingResponseModel> GetRainfallReadings([FromRoute] string stationId, [FromQuery] int? count = 10)
        {
            return await Mediator.Send(new GetRainfallReadingsRequest(stationId, count));
        }
    }
}
