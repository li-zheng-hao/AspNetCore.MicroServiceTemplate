using Microsoft.AspNetCore.Mvc;

namespace MST.User.Webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IHttpClientFactory _factory;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,IHttpClientFactory factory)
    {
        _factory = factory;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<string>Get()
    {
        var client = _factory.CreateClient("MyClient");
        var res=await client.GetStringAsync("");
        return res;
    }
}