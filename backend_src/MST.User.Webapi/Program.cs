using FluentValidation;
using FluentValidation.AspNetCore;
using MST.Infra.Shared;
using MST.User.Webapi;
using MST.User.Webapi.Startup;
using Nacos.V2.DependencyInjection;
using Serilog;
using Serilog.Context;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Tracing;
using SkyApm.Utilities.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureCustomService();
builder.Services.AddControllers().AddControllersAsServices().ConfigureCustomMvcServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomMvc();

var app = builder.Build();

Log.Logger.Information("当前运行环境:{EnvironmentName}", app.Environment.EnvironmentName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCustomMiddlewares();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// 暴露给集成测试项目
public partial class Program { }