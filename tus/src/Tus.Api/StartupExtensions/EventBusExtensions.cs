namespace Tus.Api.StartupExtensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tus.Api.Domain.Config;
using Tus.Api.Domain.Services;
using Tus.Api.Infrastructure.Services;

internal static class EventBusExtensions
{
    internal static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PubSubConfig>(configuration.GetSection(PubSubConfig.SectionName));
        services.AddTransient<IEventBus, DaprEventBus>();
        return services;
    }
}
