using Dapper;
using System.Data;
using SQLitePCL;
using Domain;

namespace Infrastructure.Seed
{
    public class SeedWeather
    {
        private readonly IDbConnection _connection;
        public SeedWeather(IDbConnection connection)
        {
            _connection = connection;
        }
        public void Seed()
        {
            Batteries.Init();

            _connection.Open();
            // Create the Weather table
            _connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Weathers (
                    RecordId INTEGER PRIMARY KEY AUTOINCREMENT,
                    CityId INTEGER NOT NULL REFERENCES Cities (CityId) ON DELETE CASCADE,
                    Time TEXT NOT NULL,
                    Wind TEXT NOT NULL,
                    Visibility TEXT NOT NULL,
                    SkyCondition TEXT NOT NULL,
                    DewPoint TEXT NOT NULL,
                    RelativeHumidity TEXT NOT NULL,
                    Pressure TEXT NOT NULL,
                    Temperature REAL NOT NULL
                );
            ");

            if (_connection.Query<string>("SELECT RecordId FROM Weathers LIMIT 1").FirstOrDefault() == null)
            {
                _connection.Execute("INSERT INTO Weathers (CityId,Time,Wind,Visibility,SkyCondition,DewPoint,RelativeHumidity,Pressure,Temperature) VALUES (1,datetime('now'),'Gentle Breeze','Clear','Haze','24–26 °C','62–72%','30.20 inHg',17.0)");
                _connection.Execute("INSERT INTO Weathers (CityId,Time,Wind,Visibility,SkyCondition,DewPoint,RelativeHumidity,Pressure,Temperature) VALUES (2,datetime('now'),'Strong','Foggy','Rain','16–18 °C','37–43%','32.10 inHg',10.0)");
                _connection.Execute("INSERT INTO Weathers (CityId,Time,Wind,Visibility,SkyCondition,DewPoint,RelativeHumidity,Pressure,Temperature) VALUES (3,datetime('now'),'Gentle Breeze','Clear','Clear','24–26 °C','62–72%','29.20 inHg',11.0)");
                _connection.Execute("INSERT INTO Weathers (CityId,Time,Wind,Visibility,SkyCondition,DewPoint,RelativeHumidity,Pressure,Temperature) VALUES (4,datetime('now'),'Low','Foggy','Haze','16–18 °C','37–43%','27.10 inHg',12.0)");
                _connection.Execute("INSERT INTO Weathers (CityId,Time,Wind,Visibility,SkyCondition,DewPoint,RelativeHumidity,Pressure,Temperature) VALUES (5,datetime('now'),'Breeze','Foggy','Rain','24–26 °C','62–72%','26.80 inHg',20.0)");

                _connection.Close();
            }
        }
    }
}
