using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Consumer.Wompi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastLowTemperatureController : ControllerBase
    {
        private static readonly IList<WeatherForecast> weather = new List<WeatherForecast>();

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return weather;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WeatherForecast value)
        {
            try
            {
                weather.Add(value);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}