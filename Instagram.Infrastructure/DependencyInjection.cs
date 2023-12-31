﻿using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence.DapperRepositories;
using Instagram.Application.Common.Interfaces.Persistence.EfRepositories;
using Instagram.Application.Common.Interfaces.Persistence.TemporaryRepositories;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Infrastructure.Authentication;
using Instagram.Infrastructure.Persistence.Connections;
using Instagram.Infrastructure.Persistence.Dapper;
using Instagram.Infrastructure.Persistence.Dapper.Repositories;
using Instagram.Infrastructure.Persistence.EF;
using Instagram.Infrastructure.Persistence.EF.Repositories;
using Instagram.Infrastructure.Services;
using Instagram.Infrastructure.Services.FileDownloaderService;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

    private static IServiceCollection AddPersistence(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        var dbConnections = new DbConnections();
        configuration.Bind(DbConnections.SectionName, dbConnections);
        services.AddSingleton(dbConnections);

        services.AddScoped<DapperContext>();
        services.AddDbContext<EfContext>(options => options.UseNpgsql(dbConnections.Postgres));

        services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();
        services.AddScoped<IEfUserRepository, EfUserRepository>();
        services.AddScoped<IEfPostRepository, EfPostRepository>();
        services.AddScoped<IDapperUserRepository, DapperUserRepository>();
        services.AddScoped<IDapperPostRepository, DapperPostRepository>();
        
        return services;
    }

    private static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IJwtTokenValidator, JwtTokenValidator>();
        services.AddSingleton<IJwtTokenHasher, JwtTokenHasher>();
        services.AddSingleton<IJwtTokenHasher, JwtTokenHasher>();
        services.AddSingleton<IPasswordHasher<object>, PasswordHasher<object>>();
        services.AddSingleton<RsaKeyGenerator>();
        
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new RsaSecurityKey(RsaKeyGenerator.RsaKey)
                };
                options.MapInboundClaims = false;
            });

        return services;
    }

    private static IServiceCollection AddFileDownloader(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.Configure<FileDownloaderSettings>(configuration.GetSection(FileDownloaderSettings.SectionName));
        services.AddSingleton<FileDownloaderSettings>();

        services.AddSingleton<IFileDownloader, FileDownloader>();

        return services;
    }
}