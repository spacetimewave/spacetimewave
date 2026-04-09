using Application.Services;
using Microsoft.AspNetCore;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Scalar.AspNetCore;
using API.Configuration;
using API.Controllers;

/*
Using directive is unnecessary.IDE0005
namespace Microsoft.AspNetCore
*/

// var builder = WebApplication.CreateSlimBuilder(args);
var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

var environment = builder.Environment.EnvironmentName;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

builder.Services
    .AddInfrastructure()
    .AddApplication()
    .AddPayments(configuration)
    .ConfigureCors(configuration)
    .ConfigureOpenApi()
    .AddSwaggerGenAuthentication(configuration)
    .AddControllers();

builder.Configuration.AddConfiguration(configuration);
builder.Services.ConfigureSettings(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);

var app = builder.Build();

app.UseCors();

if (app.Environment.EnvironmentName != "Local")
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.AddExceptionHandler();

app.MapControllers();
app.MapTodosEndpoints();
app.MapPaymentEndpoints();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Run();
