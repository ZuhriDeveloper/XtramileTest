using Application.Models;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator Mediator;
        protected readonly ILogger<BaseController> Logger;

        public BaseController(IMediator mediator, ILogger<BaseController> logger)
        {
            Logger = logger;
            Mediator = mediator;
        }

        protected async Task<T2> GetQueryResult<T1, T2>(T1 query)
            where T1 : class
            where T2 : class
        {
            try
            {
                var client = Mediator.CreateRequestClient<T1>();
                var response = await client.GetResponse<T2>(query);

                return response.Message;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        protected async Task<ActionResult> GetQueryResultResponse<T>(T query) where T : class
        {
            var client = Mediator.CreateRequestClient<T>();
            var response = await client.GetResponse<QueryResult>(query);

            if (response.Message?.IsSuccessful == true)
            {
                return new OkObjectResult(response.Message);
            }
            return new BadRequestObjectResult(response.Message);
        }

        protected Task Send<T>(T command) where T : class
        {
            return Mediator.Send(command);
        }
    }
}
