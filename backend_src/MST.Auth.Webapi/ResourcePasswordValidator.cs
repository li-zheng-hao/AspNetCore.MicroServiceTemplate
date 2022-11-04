using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using MST.Infra.Model;

namespace MST.Auth.Webapi;

public class ResourcePasswordValidator: IResourceOwnerPasswordValidator
{
    private readonly IFreeSql _freeSql;
    private readonly ILogger<ResourcePasswordValidator> _logger;

    public ResourcePasswordValidator(ILogger<ResourcePasswordValidator> logger, IFreeSql freeSql)
    {
        _freeSql = freeSql;
        _logger = logger;
    }
    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
       var user= _freeSql.Select<Users>()
            .Where(it => it.UserName == context.UserName && context.Password.ToSha256() == it.Password).First();
        // 自己去数据库判断账号密码是否正确,并且从数据库里返回用户的个人信息放到claims字段 
        // 这里还可以加入黑名单机制
        if (user is not null)
        {
            context.Result = new GrantValidationResult(
                subject: "userInfo",
                authenticationMethod: OidcConstants.AuthenticationMethods.Password,
                claims: GetUserClaims(user));
        }
        else
        {
            //验证失败
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "用户名密码错误");
            // context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "你是黑名单!");
        }
    }

    //可以根据需要设置相应的Claim/需要实现IProfileService接口
    private Claim[] GetUserClaims(Users user)
    {
        return new Claim[]
        {
            new Claim("userId",user.Id.ToString()),
            new Claim(JwtClaimTypes.Name,user.UserName),
            new Claim(JwtClaimTypes.Role,user.Role)
        };
    }
}