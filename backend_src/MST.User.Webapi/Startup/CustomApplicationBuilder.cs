using Microsoft.AspNetCore.Authentication.JwtBearer;
using MST.Infra.Rpc.Rest;
using MST.Infra.Shared;
using Refit;

namespace MST.User.Webapi.Startup;

public static class CustomApplicationBuilder
{
    public static WebApplicationBuilder ConfigureCustomService(this WebApplicationBuilder builder)
    {
        builder.Services.AddCustomSerilog(builder.Host,builder.Configuration,builder.Environment);
        builder.Services.AddCustomCors();
        // builder.Services.AddCustomCAP(builder.Configuration);
        builder.Services.AddCustomSwaggerGen();
        var policies = PollyPolicyManager.GenerateDefaultPolicies(builder.Environment);
        builder.Services.AddCustomRefitClient<IAuthRestClient>(policies,ServiceConsts.AuthServiceName);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//ids颁发的是jwt token，这里要填对应的方案
            .AddIdentityServerAuthentication(option =>
            {
                option.Authority = "http://localhost:5000"; // AuthService的地址
                option.ApiName = "UserApi"; // 和ids上配置的ApiResource要一样
                option.RequireHttpsMetadata = false;
            });
        return builder;
    }
    public static IMvcBuilder ConfigureCustomMvcService(this IMvcBuilder builder)
    {
        builder.AddCustomJson();
        // todo 取消null检查
        return builder;
    }
}