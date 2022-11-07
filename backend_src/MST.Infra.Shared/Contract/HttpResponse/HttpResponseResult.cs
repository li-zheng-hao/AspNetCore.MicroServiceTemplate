namespace MST.Infra.Shared.Contract.HttpResponse;

public class HttpResponseResult
{
    /// <summary>
    ///     返回编码
    /// </summary>
    public int code { get; set; } = ResponseCode.SUCCESS;
    /// <summary>
    /// 返回消息
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 返回数据
    /// </summary>
    public object data { get; set; }
    /// <summary>
    ///     状态码
    /// </summary>
    public bool success { get; set; } = true;

    public static HttpResponseResult Failure(string msg,int code=ResponseCode.OTHER_ERROR)
    {
        return new HttpResponseResult() { code = code, msg = msg,success=false };
    }
    public static HttpResponseResult Failure(string msg,object data,int code=ResponseCode.OTHER_ERROR)
    {
        return new HttpResponseResult() { code = code, msg = msg,success=false,data = data};
    }
    public static HttpResponseResult Success(object data)
    {
        return new HttpResponseResult() { data=data };
    }
}