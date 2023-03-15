using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class WeatherDto
    {
        public string Location { get; set; }
        public string Time { get; set; }
        public string Wind { get; set; }
        public string Visibility { get; set; }
        public string SkyCondition { get; set; }
        public double TemperatureCelcius { get; set; }
        public double TemperatureFahrenheit { get; set; }
        public string DewPoint { get; set; }
        public string RelativeHumidity { get; set; }
        public string Pressure { get; set; }
    }
}
