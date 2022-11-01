using IdentityServer4.Models;

namespace MST.Auth.Webapi;

public class ClientConfig
{
    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>()
        {
            // 浏览器应用
            new Client()
            {
                ClientId = "browser",
                ClientSecrets = {
                    new Secret( "browser".Sha256() )
                },
                // 使用用户名密码登录 还需要提交用户的用户名和密码
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                // 这个机器能够访问的api
                AllowedScopes = {
                    "Scope1"
                },
                //RefreshToken的最长生命周期
                // AbsoluteRefreshTokenLifetime = 2592000,

                //RefreshToken生命周期以秒为单位。默认为1296000秒
                SlidingRefreshTokenLifetime = 2592000,//以秒为单位滑动刷新令牌的生命周期。

                //刷新令牌时，将刷新RefreshToken的生命周期。RefreshToken的总生命周期不会超过AbsoluteRefreshTokenLifetime。
                RefreshTokenExpiration = TokenExpiration.Sliding,

                //AllowOfflineAccess 允许使用刷新令牌的方式来获取新的令牌
                AllowOfflineAccess = true,
            },
            // 控制台应用
            new Client()
            {
                ClientId = "consoleapp",
                ClientSecrets = {
                    new Secret( "consoleapp".Sha256() )
                },
                // 使用用户名密码登录 还需要提交用户的用户名和密码
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                // 这个机器能够访问的api
                AllowedScopes = {
                    "Scope2"
                },
                //RefreshToken的最长生命周期
                // AbsoluteRefreshTokenLifetime = 2592000,

                //RefreshToken生命周期以秒为单位。默认为1296000秒
                SlidingRefreshTokenLifetime = 2592000,//以秒为单位滑动刷新令牌的生命周期。

                //刷新令牌时，将刷新RefreshToken的生命周期。RefreshToken的总生命周期不会超过AbsoluteRefreshTokenLifetime。
                RefreshTokenExpiration = TokenExpiration.Sliding,

                //AllowOfflineAccess 允许使用刷新令牌的方式来获取新的令牌
                AllowOfflineAccess = true,
            },
            // 服务内部非用户调用使用这个
            new Client()
            {
                ClientId = "internal",
                ClientSecrets = {
                    new Secret( "internal".Sha256() )
                },
                // 使用用户名密码登录 还需要提交用户的用户名和密码
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // 这个机器能够访问的api
                AllowedScopes = {
                    "All"
                },
                //RefreshToken的最长生命周期
                // AbsoluteRefreshTokenLifetime = 2592000,

                //RefreshToken生命周期以秒为单位。默认为1296000秒
                SlidingRefreshTokenLifetime = Int32.MaxValue,//以秒为单位滑动刷新令牌的生命周期。

                //刷新令牌时，将刷新RefreshToken的生命周期。RefreshToken的总生命周期不会超过AbsoluteRefreshTokenLifetime。
                RefreshTokenExpiration = TokenExpiration.Sliding,

                //AllowOfflineAccess 允许使用刷新令牌的方式来获取新的令牌
                AllowOfflineAccess = true,
            }
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>()
        {
            new ApiScope("All"),
            new ApiScope("UserService"),
            new ApiScope("GoodsService"),
            new ApiScope("OrderService"),
        };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>()
        {
            new ApiResource("UserService", "用户服务")
            {
                Scopes = { "UserService","All" }
            },
            new ApiResource("GoodsService", "商品服务")
            {
                Scopes = { "GoodsService","All" }
            },
            new ApiResource("OrderService", "订单服务")
            {
                Scopes = { "OrderService","All" }
            }
        };
    }
    /// <summary>
    /// 用不上了,因为设置全部都包含
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }
  
}