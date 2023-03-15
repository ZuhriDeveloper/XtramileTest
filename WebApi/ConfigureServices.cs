using Application.Consumers;
using MassTransit;

namespace WebApi
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebAPIServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMediator(x =>
            {
                x.AddConsumer<LocationQueryConsumer>();
                x.AddConsumer<WeatherQueryConsumer>();
            });

            return services;
        }
    }
}
