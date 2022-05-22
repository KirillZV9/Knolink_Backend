using PomogatorAPI.Interfaces;
using PomogatorAPI.Models;
using PomogatorAPI.Repositories;

string appCredPath = @"C:\Users\zu-kl\source\repos\PomogatorApp\PomogatorAPI\FirebaseCred.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", appCredPath);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomer, CustomerRepository>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

