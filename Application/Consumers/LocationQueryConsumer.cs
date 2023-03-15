using Application.Queries;
using Application.Repositories;
using MassTransit;
using Application.Models;

namespace Application.Consumers
{
    public class LocationQueryConsumer : IConsumer<GetListCountryQuery>,IConsumer<GetListCityByCountryIdQuery>
    {
        private readonly ILocationRepository _locationRepository;
        public LocationQueryConsumer(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task Consume(ConsumeContext<GetListCountryQuery> context)
        {
            try
            {
                var result = await _locationRepository.GetListCountry();
                await context.RespondAsync(QueryResult.Ok(result));
            }
            catch (Exception ex)
            {
                await context.RespondAsync(QueryResult.Error(ex.Message));
            }
        }

        public async Task Consume(ConsumeContext<GetListCityByCountryIdQuery> context)
        {
            try
            {
                var result = await _locationRepository.GetListCityByCountryId(context.Message.CountryId);
                await context.RespondAsync(QueryResult.Ok(result));
            }
            catch (Exception ex)
            {
                await context.RespondAsync(QueryResult.Error(ex.Message));
            }
        }
    }
}
