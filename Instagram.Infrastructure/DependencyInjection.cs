using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Persistence.RedisRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Domain.Configurations;
using Instagram.Infrastructure.Persistence.Dapper;
using Instagram.Infrastructure.Persistence.Dapper.Repositories;
using Instagram.Infrastructure.Persistence.EF;
using Instagram.Infrastructure.Persistence.EF.Repositories;
using Instagram.Infrastructure.Persistence.Redis;
using Instagram.Infrastructure.Persistence.Redis.Repositories;
using Instagram.Infrastructure.Persistence.S3;
using Instagram.Infrastructure.Services;
using Instagram.Infrastructure.Services.FileProviders;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Instagram.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, AppConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddAuth(configuration);
        
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, AppConfiguration configuration
        )
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddSingleton<DapperContext>();
        services.AddScoped<IDapperUserRepository, DapperUserRepository>();
        services.AddScoped<IDapperPostRepository, DapperPostRepository>();
        
        services.AddDbContext<EfContext>(options => options.UseNpgsql(configuration.Connections.Postgres.Url));
        services.AddScoped<IEfUserRepository, EfUserRepository>();
        services.AddScoped<IEfPostRepository, EfPostRepository>();

        services.AddSingleton<RedisContext>();
        services.AddScoped<IRedisTokenRepository, RedisTokenRepository>();

        services.AddSingleton<S3Context>();
        services.AddSingleton<FileProvider, S3Provider>();
        
        return services;
    }
    
    private static IServiceCollection AddAuth(this IServiceCollection services, AppConfiguration configuration)
    {
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                };
                
                options.MapInboundClaims = false;
                options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    configuration.Oidc.ConfigurationUri,
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
                        var accessToken = context.Request.Headers.Authorization.ToString()["Bearer ".Length..].Trim();
                        
                        if (tokenPair is null || tokenPair.AccessToken != accessToken)
                            context.Fail(string.Empty);
                    }
                };
            });

        return services;
    }
}