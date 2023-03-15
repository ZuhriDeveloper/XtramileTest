using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Weather
    {
        public int RecordId { get; set; }
        public int CityId { get; set; }
        public DateTime Time { get; set; }
        public string Wind { get; set; }
        public string Visibility { get; set; }
        public string SkyCondition { get; set; }
        public double Temperature { get; set; }
        public string DewPoint { get; set; }
        public string RelativeHumidity { get; set; }
        public string Pressure { get; set; }
    }
}
