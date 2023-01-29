using Microsoft.AspNetCore.Mvc;

namespace Asp.Net7.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {


        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "xx")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}