using Microsoft.AspNetCore.Authentication.JwtBearer;
using MST.Infra.Configuration;
using MST.Infra.Rpc.Rest;
using MST.Infra.Shared;
using MST.Infra.Shared.Attribute;
using MST.Infra.Task;
using MST.User.Repository.UserRepository;
using MST.User.Service;
using Quickwire;
using Refit;
using Serilog.Context;
using SkyApm.Tracing;

namespace MST.User.Webapi.Startup;

public static class CustomApplicationBuilder
{
    public static WebApplicationBuilder ConfigureCustomService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScheduler();
        builder.Services.AddCustomSerilog(builder.Host,builder.Configuration,builder.Environment);
        builder.Services.AddNacosConfigurationCenter(builder.Configuration);
        builder.Services.AddNacosServiceDiscoveryCenter(builder.Configuration);
        builder.Services.AddCustomCors();
        builder.Services.AddCustomCAP(builder.Configuration);
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
        builder.Services.AddFreeSql(builder.Configuration, typeof(UserRepository).Assembly);
        builder.Services.ScanAssembly(typeof(UserService).Assembly,it=>true);
        builder.Services.ScanAssembly(typeof(UserRepository).Assembly,it=>true);
        builder.Services.ScanAssembly(typeof(CommonOptions).Assembly,it=>true);
        builder.Services.ScanAssembly(typeof(Program).Assembly,it=>true);
        builder.Services.AddFreeRepository(null, typeof(Program).Assembly);
        return builder;
    }
    public static IMvcBuilder ConfigureCustomMvcServices(this IMvcBuilder builder)
    {
        builder.AddCustomJson();
        // todo 取消null检查
        return builder;
    }
    public static WebApplication UseCustomMiddlewares(this WebApplication app)
    {
        // 输出到elasticsearch的日志加上skywalking的traceid，方便请求过滤
        app.Use(async (context, next) =>
        {
            var accessors = context.RequestServices.GetService<IEntrySegmentContextAccessor>();
            using var _=LogContext.PushProperty("TraceId", accessors?.Context.TraceId);
            await next();
        });
        app.Use(async (context, next) =>
        {
            TransactionalAttribute.SetServiceProvider(context.RequestServices);
            await next();
        });
        return app;
    }
}