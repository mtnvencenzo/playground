namespace Tus.Api.StartupExtensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

internal static class CorsExtensions
{
    internal static IServiceCollection AddHttpCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("origin-policy", corsBuilder =>
            {
                corsBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders(tusdotnet.Helpers.CorsHelper.GetExposedHeaders())
                    .WithExposedHeaders(configuration["TusApi:DocumentIdHeaderName"])
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(600));
            });
        });

        return services;
    }
}
