using Application.Repositories;
using Dapper;
using Domain;
using System.Data;

namespace Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IDbConnection _connection;
        public LocationRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public Task<City> GetCityById(int CityId)
        {
            var query = @"SELECT * FROM Cities WHERE CityId = @CityId";
            return _connection.QuerySingleOrDefaultAsync<City>(query, new { CityId });
        }

        public Task<Country> GetCountryById(int CountryId)
        {
            var query = @"SELECT * FROM Countries WHERE CountryId = @CountryId";
            return _connection.QuerySingleOrDefaultAsync<Country>(query, new { CountryId });
        }

        public Task<IEnumerable<City>> GetListCityByCountryId(int CountryId)
        {
            var query = @"SELECT * FROM Cities WHERE CountryId = @CountryId";
            return _connection.QueryAsync<City>(query, new { CountryId });
        }

        public Task<IEnumerable<Country>> GetListCountry()
        {
            var query = @"SELECT * FROM Countries";
            return _connection.QueryAsync<Country>(query);
        }
    }
}
