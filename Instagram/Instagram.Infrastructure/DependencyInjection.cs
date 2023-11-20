using Instagram.Application.Common.Interfaces.Authentication;
using Instagram.Application.Common.Interfaces.Persistence;
using Instagram.Application.Common.Interfaces.Services;
using Instagram.Infrastructure.Authentication;
using Instagram.Infrastructure.Persistence;
using Instagram.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Instagram.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}