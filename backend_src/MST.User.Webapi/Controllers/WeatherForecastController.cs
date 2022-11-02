using Microsoft.AspNetCore.Mvc;
using MST.Infra.Rpc.Rest;

namespace MST.User.Webapi.Controllers;

[ApiController]
[Route("user/[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IAuthRestClient _authRestClient;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IAuthRestClient authRestClient)
    {
        _authRestClient = authRestClient;
        _logger = logger;
    }

    [HttpGet]
    public async Task<string> Get()
    {
        // var client = _factory.CreateClient("MyClient");
        // var res = await client.GetStringAsync("");
        return "1";
    }

    [HttpPost]
    public async Task<string> TestLogin()
    {
        var res=await _authRestClient.LoginAsync(new LoginRequestDto()
        {
            client_id = "browser", client_secret = "browser", grant_type = "password", password = "lizhenghao",
            username = "lizhenghao"
        });
        if (res.IsSuccessStatusCode == false)
        {
            return res.Error.Message;
        }
        else
        {
            return res.Content.ToJsonString();
        }
    }
}