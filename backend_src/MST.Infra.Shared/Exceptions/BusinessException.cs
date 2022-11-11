using System.Net;

namespace MST.Infra.Shared.Exceptions;

public class BusinessException:Exception
{
     /// <summary>
     /// 业务错误代码
     /// </summary>
     public int BusinessCode { get; set; } = 500;
     public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.InternalServerError;
     public BusinessException(string errMsg):base(errMsg)
     { }
     /// <summary>
     /// 
     /// </summary>
     /// <param name="errMsg">错误信息</param>
     /// <param name="errCode">业务错误代码</param>
     public BusinessException(string errMsg,int errCode):this(errMsg)
     {
          BusinessCode = errCode;
     }
     /// <summary>
     /// 
     /// </summary>
     /// <param name="errMsg">错误信息</param>
     /// <param name="errCode">业务错误代码</param>
     /// <param name="httpCode">http状态码 默认500</param>
     public BusinessException(string errMsg,int errCode,HttpStatusCode httpCode):this(errMsg,errCode)
     {
          HttpCode= httpCode;
     }
}