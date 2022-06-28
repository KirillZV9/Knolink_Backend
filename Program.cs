using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PomogatorAPI.Interfaces;
using PomogatorAPI.Models;
using PomogatorAPI.Repositories;
using System.Text;

string appCredPath = @"C:\Users\zu-kl\source\repos\PomogatorApp\PomogatorAPI\FirebaseCred.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", appCredPath);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomer, CustomerService>();

builder.Services.AddScoped<ITutor, TutorService>();

builder.Services.AddScoped<IAuth, AuthService>();

builder.Services.AddScoped<IOrder, OrderService>();

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

