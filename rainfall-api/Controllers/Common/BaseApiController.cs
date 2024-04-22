using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace rainfall_api.Controllers.Common
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private ISender _mediator = null!;

        public ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
