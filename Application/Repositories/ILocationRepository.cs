using Domain;
namespace Application.Repositories
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Country>> GetListCountry();
        Task<IEnumerable<City>> GetListCityByCountryId(int CountryId);
        Task<Country> GetCountryById(int CountryId);
        Task<City> GetCityById(int CityId);
    }
}
