using BlacklistApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Serilog;
using BlacklistApp.Services.Models;
using Serilog.Events;
using BlacklistApp.Services.Helpers;
using BlacklistApp.Services.Interfaces;
using BlacklistApp.Services.Services;
using Microsoft.AspNetCore.Authentication;
using AuthenticationService = BlacklistApp.Services.Services.AuthenticationService;
using IAuthenticationService = BlacklistApp.Services.Interfaces.IAuthenticationService;
using Serilog.Core;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
.MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting web application");
builder.Logging.ClearProviders();
builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RepositoryContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("dbconnection")));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<RepositoryContext>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddMvc().ConfigureApiBehaviorOptions(options =>
    options.InvalidModelStateResponseFactory = c =>
    {
        var errors = string.Join('\n', c.ModelState.Values.Where(v => v.Errors.Count > 0)
            .SelectMany(v => v.Errors)
            .Select(v => v.ErrorMessage));
        return new BadRequestObjectResult(new Result(false, errors, 400));
    }
);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware(typeof(GlobalErrorHandlingMiddleware));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


