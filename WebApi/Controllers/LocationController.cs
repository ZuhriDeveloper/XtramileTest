using Application.Queries;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    public class LocationController : BaseController
    {
        public LocationController(IMediator mediator, ILogger<LocationController> logger) : base(mediator, logger)
        {

        }

        [HttpGet("GetListCountry")]
        public async Task<ActionResult> GetList()
        {
            var query = new GetListCountryQuery();
            return await GetQueryResultResponse(query);
        }

        [HttpGet("GetListCityByCountry")]
        public async Task<ActionResult> GetListCityByCountry([FromQuery] GetListCityByCountryIdQuery query)
        {
            return await GetQueryResultResponse(query);
        }
    }
}
