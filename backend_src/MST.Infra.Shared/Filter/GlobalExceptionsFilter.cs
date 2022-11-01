using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MST.Infra.Shared.Contract.HttpResponse;

namespace MST.Infra.Shared.Filter;
/// <summary>
/// 接口全局异常错误日志
/// </summary>
public class GlobalExceptionsFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionsFilter> _logger;

    public GlobalExceptionsFilter(ILogger<GlobalExceptionsFilter> logger)
    {
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {
        // TODO 判断是否为自定义的业务异常
        _logger.LogError(context.Exception,$"全局异常 {context.Exception.Message}");
        HttpStatusCode status = HttpStatusCode.InternalServerError;

        //处理各种异常
        var jm = new ResponseResult
        {
            Status = false,
            Code = (int)status,
            Msg = "系统返回异常，请联系管理员进行处理！",
            Data = context.Exception
        };
        context.ExceptionHandled = true;
        context.Result = new JsonResult(jm);
    }

}


