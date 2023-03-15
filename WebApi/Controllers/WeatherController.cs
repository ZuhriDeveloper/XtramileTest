using Application.Queries;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class WeatherController : BaseController
    {
        public WeatherController(IMediator mediator, ILogger<WeatherController> logger) : base(mediator, logger)
        {

        }

        [HttpGet("GetWeatherByCity")]
        public async Task<ActionResult> GetWeatherByCity([FromQuery] GetWeatherQuery query)
        {
            return await GetQueryResultResponse(query);
        }
    }
}
