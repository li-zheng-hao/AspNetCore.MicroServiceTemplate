using MST.Infra.Shared;
using MST.User.Webapi;
using MST.User.Webapi.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureCustomService();
builder.Services.AddControllers().AddCustomJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();