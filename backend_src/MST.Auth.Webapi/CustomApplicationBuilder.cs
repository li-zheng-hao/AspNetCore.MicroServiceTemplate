using MST.Infra.Shared;
using Serilog.Context;
using SkyApm.Tracing;

namespace MST.Auth.Webapi;

public static class CustomApplicationBuilder
{
    public static WebApplicationBuilder ConfigureCustomService(this WebApplicationBuilder builder)
    {
        builder.Services.AddNacosConfigurationCenter(builder.Configuration);
        builder.Services.AddNacosServiceDiscoveryCenter(builder.Configuration);
        builder.Services.AddFreeSql(builder.Configuration, null);
        return builder;
    }
  
  
}