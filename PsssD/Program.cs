using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PsssD;
using PsssD.RabbitMQ;
using PsssD.Service;
using System.Configuration;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationContext>(
               options =>
               {
                   options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresqlConnection"));
               });


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddScoped<IRabitMQProducer, RabitMQProducer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/admin", ([FromHeader(Name = "X-Api-Key")] string apiKey) =>
{
    if (apiKey == "SuperSecretApiKey")
    {
        return Results.Ok("Hi admin!");
    }

    return Results.Unauthorized();
});

app.MapControllers();
app.Run();