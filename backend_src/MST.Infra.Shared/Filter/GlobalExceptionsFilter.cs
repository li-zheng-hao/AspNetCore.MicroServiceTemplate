using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MST.Infra.Shared.Contract.HttpResponse;
using MST.Infra.Shared.Exceptions;

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
        if (context.Exception is BusinessException)
        {
            var businessException = context.Exception as BusinessException;
            //处理各种异常
            var customEx = new HttpResponseResult
            {
                success = false,
                code = (int)businessException!.BusinessCode,
                msg = businessException.Message,
            };
            context.HttpContext.Response.StatusCode = (int)businessException.HttpCode;
            context.ExceptionHandled = true;
            context.Result = new JsonResult(customEx);
        }
        else
        {
            _logger.LogError(context.Exception,$"全局未捕获异常 {context.Exception.Message}");
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            //处理各种异常
            var uncactchedEx = new HttpResponseResult
            {
                success = false,
                code = (int)status,
                msg = "系统返回异常，请联系管理员进行处理！",
            };
            context.ExceptionHandled = true;
            context.Result = new JsonResult(uncactchedEx);
        }
        
       
    }

}


