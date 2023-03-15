using Domain;

namespace Application.Repositories
{
    public interface IWeatherRepository
    {
        Task <Weather> GetWeatherByCityId(int CityId);
    }
}
