using Microsoft.AspNetCore.Mvc;
using MST.Auth.Webapi;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.ConfigureCustomService();

builder.Services.AddSwaggerGen();
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential() //临时生成的加密证书 即公钥私钥 也可以自己弄一个然后指定路径
    .AddInMemoryApiScopes(ClientConfig.GetApiScopes()) // 指定所有Api作用域
    .AddInMemoryApiResources(ClientConfig.GetApiResources()) // 指定Api(即后台服务)所对应的作用域，两者是多对多的关系
    .AddInMemoryClients(ClientConfig.GetClients()) // 获取所有客户端信息
    .AddResourceOwnerValidator<ResourcePasswordValidator>() //用户验证
    .AddProfileService<ProfileService>(); // 把用户的个人信息字段放到Token里
// .AddInMemoryIdentityResources(ClientConfig.GetIdentityResources()); // 指定客户端允许访问的用户信息,由于我们在ProfileService内直接将所有字段都加入进去了，所以这个用不上
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// url重写 
app.Use(async (httpContext, next) =>
{
    if (httpContext.Request.Path.Value!.Contains("/auth"))
    {
        httpContext.Request.Path = httpContext.Request.Path.Value.Replace("/auth", "");
    }
    await next();
});
app.UseIdentityServer();
app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();