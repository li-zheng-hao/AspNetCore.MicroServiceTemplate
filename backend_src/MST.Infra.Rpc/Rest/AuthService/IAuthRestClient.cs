using MST.Infra.Shared.Contract.HttpResponse;
using Refit;

namespace MST.Infra.Rpc.Rest;

public interface IAuthRestClient : IRestClient
{
    /// <summary>
    ///  登录
    /// </summary>
    /// <returns></returns>
    [Post("/connect/token")]
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest);

    
}

