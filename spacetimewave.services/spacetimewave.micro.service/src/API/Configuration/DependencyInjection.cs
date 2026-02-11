using Application.Repositories;
using Application.Services;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Application.Configuration;
using API.Controllers;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Domain.Entities;
using System.Security.Cryptography;

namespace API.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"));
        services.Configure<AuthenticationSettings>(configuration.GetSection("AuthenticationSettings"));
        
        ApplicationSettings applicationSettings = configuration
            .GetSection("ApplicationSettings")
            .Get<ApplicationSettings>() ?? new ApplicationSettings();

        AuthenticationSettings authenticationSettings = configuration
            .GetSection("AuthenticationSettings")
            .Get<AuthenticationSettings>() ?? new AuthenticationSettings();

        services.AddSingleton(applicationSettings);
        services.AddSingleton(authenticationSettings);

        return services;
    }

    public static AuthenticationBuilder ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration, AuthenticationOptions? authenticationOptions = null)
    {
        AuthenticationSettings authenticationSettings = configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>() ?? new AuthenticationSettings();
        
        switch (authenticationSettings.AuthenticationType)
        {
            case AuthenticationType.JwtBearer:
                return services.ConfigureJwtBearerAuthentication(configuration, authenticationOptions);

            case AuthenticationType.None:
            default:
                return services.ConfigureNoneAuthentication();
        } 
    }

    // public static AuthenticationBuilder ConfigureMsEntraIdAuthentication(this IServiceCollection services, IConfiguration configuration, AuthenticationOptions? authOptions)
    // {
    //     AuthenticationSettings authenticationSettings = configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>() ?? new AuthenticationSettings();

    //     AzureAd azureAdSettings = authenticationSettings.AzureAd;
    //     string oAuthServerUrl = $"{azureAdSettings.Instance}{azureAdSettings.TenantId}/v2.0";
    //     return services.AddAuthentication(options =>
    //     {
    //         options.DefaultChallengeScheme = authOptions?.DefaultChallengeScheme ?? JwtBearerDefaults.AuthenticationScheme;
    //         options.DefaultAuthenticateScheme = authOptions?.DefaultAuthenticateScheme ?? JwtBearerDefaults.AuthenticationScheme;
    //     })
    //     .AddJwtBearer(options =>
    //     {
    //         options.Authority = $"{azureAdSettings.Instance}{azureAdSettings.TenantId}/v2.0";
    //         options.TokenValidationParameters = new TokenValidationParameters
    //         {
    //             ValidateIssuer = true,
    //             ValidateAudience = true, // False for multi audience scenarios
    //             ValidateLifetime = true,
    //             ValidateIssuerSigningKey = true,
    //             ValidAudience = azureAdSettings.Audience,
    //             // ValidIssuer = $"{azureAdSettings.Instance}{azureAdSettings.TenantId}/v2.0",
    //             ValidIssuers = new[]
    //             {
    //                 $"{azureAdSettings.Instance}{azureAdSettings.TenantId}/v2.0",
    //                 $"https://sts.windows.net/{azureAdSettings.TenantId}/"
    //             },
    //             NameClaimType = "name",
    //             RoleClaimType = "roles"
    //         };

    //         options.Events = new JwtBearerEvents
    //         {
    //             OnTokenValidated = context =>
    //             {
    //                 var name = context.Principal?.Identity?.Name ?? "unknown";
    //                 var email = context.Principal?.FindFirstValue("preferred_username") ?? "unknown";
    //                 Console.WriteLine($"Token validated for: {name} ({email})");
    //                 return Task.CompletedTask;
    //             },
    //             OnAuthenticationFailed = context =>
    //             {
    //                 Console.WriteLine($"Authentication failed: {context.Exception.Message}");
    //                 return Task.CompletedTask;
    //             },
    //             OnChallenge = context =>
    //             {
    //                 Console.WriteLine($"Challenging client to authenticate with Entra ID");
    //                 return Task.CompletedTask;
    //             }
    //         };
    //     });
    // }

    public static TokenValidationParameters GetJwtValidationParameters( AuthenticationSettings authenticationSettings)
    {
        List<SecurityKey> rsaPublicKeys = new List<SecurityKey> { };
        return new TokenValidationParameters
            {
                ValidIssuer = authenticationSettings.Issuer,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,

                ValidateAudience = true, // for multi-audience scenarios: false
                ValidAudience = authenticationSettings.Audience,

                ValidateLifetime = true,
                // NameClaimType = "name",
                // RoleClaimType = "roles"
            };
    }

    public static AuthenticationBuilder ConfigureJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration, AuthenticationOptions? authOptions, bool useIssuerPublicKeys = true)
    {
        AuthenticationSettings authenticationSettings = configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>() ?? new AuthenticationSettings();
        return services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; 
            options.MetadataAddress = authenticationSettings.MetadataUrl; 
            options.TokenValidationParameters = GetJwtValidationParameters(authenticationSettings);
            
            options.Events = new JwtBearerEvents
            {
                // OnChallenge = context =>
                // {
                //     context.HandleResponse(); 
                //     context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //     return Task.CompletedTask;
                // }
            };
        });
    }
    
    
    public static AuthenticationBuilder ConfigureAzureAdAuthentication(this IServiceCollection services, IConfiguration configuration, AuthenticationOptions? authOptions)
    {
        AuthenticationBuilder authBuilder = services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme = authOptions?.DefaultChallengeScheme ?? JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = authOptions?.DefaultAuthenticateScheme ?? JwtBearerDefaults.AuthenticationScheme;
        });

        authBuilder.AddMicrosoftIdentityWebApi(configuration.GetSection("AuthenticationSettings:AzureAd"));
        return authBuilder;
    }

    public static AuthenticationBuilder ConfigureNoneAuthentication(this IServiceCollection services)
    {
        return services.AddAuthentication("None")
            .AddScheme<AuthenticationSchemeOptions, AnonymousAuthenticationHandler>("None", options => { });
    }
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITodosRepository, TodosRepository>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // 3rd party Services
        services.AddProblemDetails();

        // Application Services
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITodosService, TodosService>();
        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    } 

    public static IServiceCollection AddSwaggerGenAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        AuthenticationSettings authenticationSettings = configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>() ?? new AuthenticationSettings();

        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(id => id.FullName); 
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.SecuritySchemeType.OAuth2,
                Flows = new Microsoft.OpenApi.OpenApiOAuthFlows
                {
                    AuthorizationCode = new Microsoft.OpenApi.OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(authenticationSettings.AuthorizationUrl),
                        TokenUrl = new Uri(authenticationSettings.TokenUrl),
                        Scopes = authenticationSettings.Scopes.Split(' ').ToDictionary(scope => scope, scope => scope)
                    }
                }
            });

            options.AddSecurityRequirement(doc => new Microsoft.OpenApi.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", doc),
                    []  
                }
            });
        });    

        return services;
    } 

    public static WebApplication AddExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(configure => 
            configure.Run(async context => 
                await Results.Problem()
                    .ExecuteAsync(context)
            )
        );

        return app;
    }

    public static IServiceCollection ConfigureOpenApi(this IServiceCollection services)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
        return services;
    }
}

public class AnonymousAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public AnonymousAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity("None");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "None");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}