using Application.Repositories;
using Dapper;
using Domain;
using System.Data;

namespace Infrastructure.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly IDbConnection _connection;
        public WeatherRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public Task<Weather> GetWeatherByCityId(int CityId)
        {
            var query = @"SELECT * FROM Weathers WHERE CityId = @CityId";
            return _connection.QuerySingleOrDefaultAsync<Weather>(query, new { CityId });
        }
    }
}
