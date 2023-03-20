using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebUi.Models;
using Application.Models;
using Newtonsoft.Json;
using Domain;
using MassTransit.Mediator;
using Application.Dtos;

namespace WebUi.Controllers
{
    public class WeatherForecastController : Controller
    {
        private readonly string _webApiUrl;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(IConfiguration configuration)
        {
            _configuration = configuration;
            _webApiUrl = _configuration["WebApiUrl"];
        }
        public async Task<IActionResult> Index()
        {
            var model = new WeatherForecast();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_webApiUrl + "/api/Location/GetListCountry"))
                {
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString);
                    var countries = JsonConvert.DeserializeObject<List<Country>>(responseJson.payload.ToString());
                    model.ListCountry = countries;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> GetCityOptions([FromQuery] int countryId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_webApiUrl + "/api/Location/GetListCityByCountry/?countryid=" + countryId))
                {
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString);
                    var cities = JsonConvert.DeserializeObject<List<City>>(responseJson.payload.ToString());
                    var listDropdown = new List<CityDto>();
                    foreach (City city in cities)
                    {
                        var dto = new CityDto { id = city.CityId, name = city.CityName };
                        listDropdown.Add(dto);
                    }
                    return Json(listDropdown);
                }
            }
        }

        public async Task<string> GetWeatherInfo([FromQuery] int cityId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_webApiUrl + "/api/Weather/GetWeatherByCity/?cityid=" + cityId))
                {
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString);
                    
                    return responseJson.payload.ToString();
                }
            }
        }
    }
}
