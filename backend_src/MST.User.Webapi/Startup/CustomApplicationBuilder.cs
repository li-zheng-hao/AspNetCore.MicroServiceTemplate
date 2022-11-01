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
        return builder;
    }
    public static IMvcBuilder ConfigureCustomMvcService(this IMvcBuilder builder)
    {
        builder.AddCustomJson();
        // todo 取消null检查
        return builder;
    }
}