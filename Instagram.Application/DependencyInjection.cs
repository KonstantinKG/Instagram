using System.Reflection;
using FluentValidation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Instagram.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services, 
        ConfigurationManager configuration
        )
    {
        services.AddApplicationSettings(configuration);
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddPipeline();
        
        return services;
    }
    
    private static IServiceCollection AddApplicationSettings(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<ApplicationSettings>(configuration.GetSection(ApplicationSettings.SectionName));
        return services;
    }

    private static IServiceCollection AddPipeline(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
            (t.Name.EndsWith("Handler") || t.Name.EndsWith("Pipeline")) &&
            t is { IsAbstract: false, IsInterface: false });
        foreach (var t in types)
        {
            services.AddScoped(t);
        }

        return services;
    }
}