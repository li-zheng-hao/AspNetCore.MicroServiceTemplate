using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MST.Infra.Rpc.Rest;
using MST.Infra.Shared.Contract.HttpResponse;
using MST.User.Contract;
using SkyApm.Diagnostics.MSLogging;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using ILogger = SkyApm.Logging.ILogger;

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
    private readonly IEntrySegmentContextAccessor _segContext;
    private readonly ITracingContext _tracingContext;
    private readonly ILoggerFactory _skyapmlogger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,ILoggerFactory skyapmlogger, IAuthRestClient authRestClient,ITracingContext tracingContext, IEntrySegmentContextAccessor  segContext)
    {
        _skyapmlogger = skyapmlogger;
        _tracingContext = tracingContext;
        _segContext = segContext;
        _authRestClient = authRestClient;
        _logger = logger;
    }
    /// <summary>
    /// 随便测试
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    [HttpGet]
    public string HelloWorld(string msg)
    {
        //获取全局的skywalking的TracId
        var TraceId = _segContext.Context.TraceId;
        Console.WriteLine($"TraceId={TraceId}");
        _logger.LogInformation("测试Serilog写入Skywalking");
        var logger = _skyapmlogger.CreateLogger("logger");
        var logger2 = _skyapmlogger.CreateLogger(typeof(SkyApmLogger));
        logger.LogInformation("测试官方的Logger");
        logger2.LogInformation("测试官方的Logger");
        _segContext.Context.Span.AddLog(LogEvent.Message($"UserService调用---Worker running at: {DateTime.Now}"));
        return msg;
    }
    /// <summary>
    /// 测试登录
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 测试管理员角色校验
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "admin")]
    [HttpPost]
    public string TestAuth()
    {
        return "管理员验证成功";
    }
    /// <summary>
    /// 测试管理员角色校验
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public string TestModelValidation(TestDto dto)
    {
        return "DTO校验通过";
    }
}