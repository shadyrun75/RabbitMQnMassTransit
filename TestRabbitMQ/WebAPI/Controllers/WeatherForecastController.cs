using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<WeatherForecast> _client;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IPublishEndpoint publishEndpoint,
            IRequestClient<WeatherForecast> client)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _client = client;
        }

        //[HttpGet]
        //public async Task<IEnumerable<WeatherForecast>> Get()
        //{
        //    try
        //    {

        //    }
        //    catch
        //    {

        //    }
        //}

        [HttpPost]
        public async Task<ActionResult<BasicResponse>> Post([FromBody] WeatherForecast value)
        {
            try
            {
                //await publishEndpoint.Publish<WeatherForecast>(value);

                //using (var request = _client.Create(value))
                //{
                //    var response = await request.GetResponse<WeatherForecastPostResponse>();
                //    return response.Message;
                //};

                var result = await _client.GetResponse<BasicResponse>(value);
                return result.Message;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private void SomeFunc(PublishContext<WeatherForecast> obj)
        {
            throw new NotImplementedException();
        }

    }
}