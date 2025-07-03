namespace Tus.Api.StartupExtensions;

using Microsoft.Extensions.DependencyInjection;
using Tus.Api.Domain.Config;
using Tus.Api.Domain.Services;
using Tus.Api.Infrastructure.Services;

internal static class StorageBusExtensions
{
    internal static IServiceCollection AddStorageBus(this IServiceCollection services)
    {
        services.AddTransient<IStorageBus, DaprStorageBus>();
        return services;
    }
}
