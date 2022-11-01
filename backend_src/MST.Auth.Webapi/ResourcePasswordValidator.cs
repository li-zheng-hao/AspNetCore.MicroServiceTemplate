using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace MST.Auth.Webapi;

public class ResourcePasswordValidator: IResourceOwnerPasswordValidator
{
    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        // 自己去数据库判断账号密码是否正确,并且从数据库里返回用户的个人信息放到claims字段 
        // 这里还可以加入黑名单机制
        if (context.UserName == "lizhenghao" && context.Password == "lizhenghao")
        {
            context.Result = new GrantValidationResult(
                subject: "userInfo",
                authenticationMethod: OidcConstants.AuthenticationMethods.Password,
                claims: GetUserClaims());
        }
        else
        {
            //验证失败
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "用户名密码错误");
            // context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "你是黑名单!");
        }
    }

    //可以根据需要设置相应的Claim/需要实现IProfileService接口
    private Claim[] GetUserClaims()
    {
        return new Claim[]
        {
            new Claim("userId","123456"),
            new Claim(JwtClaimTypes.Name,"李正浩"),
            new Claim(JwtClaimTypes.Role,"admin"),
            new Claim("自定义属性","自定义结果")
        };
    }
}