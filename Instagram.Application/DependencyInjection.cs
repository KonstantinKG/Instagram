using System.Reflection;
using FluentValidation;
using Instagram.Application.Common.Markers;
using Microsoft.Extensions.DependencyInjection;

namespace Instagram.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddPipeline();
        
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