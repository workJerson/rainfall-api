using Microsoft.AspNetCore.Mvc;
using rainfall_api.Controllers.Common;
using rainfall_api.Dtos;
using rainfall_api.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace rainfall_api.Controllers
{
    public class RainFallController : BaseApiController
    {
        /// <summary>
        /// Get rainfall readings by station Id
        /// </summary>
        /// <operationId>get-rainfall</operationId>
        /// <remarks>Retrieve the latest readings for the specified stationId</remarks>
        /// <param name="stationId">The id of the reading station.</param>
        /// <param name="count">The number of readings to return.</param>
        /// <response code="200">A list of rainfall readings successfully retrieved</response>  
        /// <response code="400">Invalid request</response>  
        /// <response code="404">No readings found for the specified stationId</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("id/{stationId}/readings")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(RainfallReadingResponseModel))]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<RainfallReadingResponseModel> GetRainfallReadings([FromRoute] string stationId, [FromQuery] int? count = 10)
        {
            return await Mediator.Send(new GetRainfallReadingsRequest(stationId, count));
        }
    }
}
