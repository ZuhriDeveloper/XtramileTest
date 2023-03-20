using Domain;

namespace WebUi.Models
{
    public class WeatherForecast
    {
        public WeatherForecast()
        {
            ListCountry = new List<Country>();
        }
        public List<Country> ListCountry { get; set; }
    }


}
