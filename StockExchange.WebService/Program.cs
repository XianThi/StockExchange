using Microsoft.Data.SqlClient;
using SoapCore;
using StockExchange.BAL.Services;
using StockExchange.Core.Interfaces.Repositories;
using StockExchange.Core.Interfaces.Services;
using StockExchange.DAL;
using StockExchange.DAL.Repositories;
using System.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDbConnection>(db => new SqlConnection(connectionString));
builder.Services.AddHostedService<StockTaskService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockService,StockService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();


builder.Services.AddSoapCore();
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var databaseService = services.GetRequiredService<IDatabaseService>();
        await databaseService.SeedDataAsync();
        Console.WriteLine("Seed data başarılı!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seed data hatası: {ex.Message}");
    }
}

app.UseSoapEndpoint<IStockService>("/Service.svc", new SoapEncoderOptions());
app.MapGet("/", () => "Hello World!");

app.Run();
