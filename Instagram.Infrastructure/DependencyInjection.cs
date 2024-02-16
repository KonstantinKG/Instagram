using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Persistence.RedisRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Infrastructure.Persistence.Common;
using Instagram.Infrastructure.Persistence.Dapper;
using Instagram.Infrastructure.Persistence.Dapper.Repositories;
using Instagram.Infrastructure.Persistence.EF;
using Instagram.Infrastructure.Persistence.EF.Repositories;
using Instagram.Infrastructure.Persistence.Redis;
using Instagram.Infrastructure.Persistence.Redis.Repositories;
using Instagram.Infrastructure.Services;
using Instagram.Infrastructure.Services.FileDownloaderService;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Instagram.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddPersistence(configuration);
        services.AddAuth(configuration);
        services.AddFileDownloader(configuration);
        
        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration
        )
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        var dbConnections = new DbConnections();
        configuration.Bind(DbConnections.SectionName, dbConnections);
        services.AddSingleton(Options.Create(dbConnections));

        services.AddSingleton<DapperContext>();
        services.AddDbContext<EfContext>(options => options.UseNpgsql(dbConnections.Postgres));

        services.AddSingleton<RedisContext>();
        services.AddScoped<IRedisTokenRepository, RedisTokenRepository>();
        
        services.AddScoped<IEfUserRepository, EfUserRepository>();
        services.AddScoped<IEfPostRepository, EfPostRepository>();
        services.AddScoped<IDapperUserRepository, DapperUserRepository>();
        services.AddScoped<IDapperPostRepository, DapperPostRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration
        )
    {
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false
                };
                
                options.MapInboundClaims = false;
                options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    configuration.GetValue<string>("AuthenticationServerOpenIdConfigurationUri"),
                    new OpenIdConnectConfigurationRetriever(),
                    new HttpDocumentRetriever()
                );
                
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var redisTokenRepository =
                            context.HttpContext.RequestServices.GetRequiredService<IRedisTokenRepository>();

                        var sessionId = context.Principal?.Claims?.FirstOrDefault(x => x.Type == "sid")?.Value;
                        if (sessionId is null)
                        {
                            context.Fail(string.Empty);
                            return;
                        }

                        var tokenPair = await redisTokenRepository.GetSessionTokens(sessionId);
                        var accessToken = context.Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
                        
                        if (tokenPair is null || tokenPair.AccessToken != accessToken)
                            context.Fail(string.Empty);
                    }
                };
            });

        return services;
    }
    
    private static IServiceCollection AddFileDownloader(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.Configure<FileDownloaderSettings>(configuration.GetSection(FileDownloaderSettings.SectionName));
        services.AddSingleton<IFileDownloader, FileDownloader>();

        return services;
    }
}