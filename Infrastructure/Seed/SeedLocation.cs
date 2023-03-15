using Dapper;
using System.Data;
using SQLitePCL;
using Domain;

namespace Infrastructure.Seed
{
    public class SeedLocation
    {
        private readonly IDbConnection _connection;
        public SeedLocation(IDbConnection connection)
        {
            _connection = connection;
        }
        public void Seed()
        {
            Batteries.Init();

            _connection.Open();
            // Create the Countries table
            _connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Countries (
                    CountryId INTEGER PRIMARY KEY AUTOINCREMENT,
                    CountryName TEXT NOT NULL
                );
            ");

            // Create the Cities table
            _connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Cities  (
                    CityId INTEGER PRIMARY KEY AUTOINCREMENT,
                    CityName TEXT NOT NULL,
                    CountryId INTEGER NOT NULL REFERENCES Countries (CountryId) ON DELETE CASCADE
                );
            "
            );

            if (_connection.Query<string>("SELECT CountryName FROM Countries LIMIT 1").FirstOrDefault() == null)
            {
                _connection.Execute("INSERT INTO Countries (CountryName) VALUES ('USA')");
                _connection.Execute("INSERT INTO Countries (CountryName) VALUES ('Canada')");
                _connection.Execute("INSERT INTO Countries (CountryName) VALUES ('Mexico')");

                _connection.Execute("INSERT INTO Cities (CityName, CountryId) VALUES ('New York', 1)");
                _connection.Execute("INSERT INTO Cities (CityName, CountryId) VALUES ('Los Angeles', 1)");
                _connection.Execute("INSERT INTO Cities (CityName, CountryId) VALUES ('Toronto', 2)");
                _connection.Execute("INSERT INTO Cities (CityName, CountryId) VALUES ('Vancouver', 2)");
                _connection.Execute("INSERT INTO Cities (CityName, CountryId) VALUES ('Mexico City', 3)");
            }

            _connection.Close();
        }
    }
}
