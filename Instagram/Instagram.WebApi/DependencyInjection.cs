using Instagram.WebApi.Common.Errors;
using Instagram.WebApi.Common.Mappings;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Instagram.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMappings();
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, InstagramProblemDetailsFactory>();
        return services;
    }
}