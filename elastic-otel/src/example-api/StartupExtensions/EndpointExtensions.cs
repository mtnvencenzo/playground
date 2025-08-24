namespace Example.Api.StartupExtensions;

using Example.Api.Apis.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Hosting;

internal static class EndpointExtensions
{
    internal static WebApplication UseApplicationEndpoints(this WebApplication app)
    {
        var rootApi = app.UseDefaultEndpoints();
        rootApi.MapSubscribeHandler();

        var exampleApi = app.NewVersionedApi("Example")
            .MapGroup("api/v{version:apiVersion}")
            .HasApiVersion(1.0);

        exampleApi.MapHealthApiV1();

        return app;
    }

    private static WebApplication UseDefaultEndpoints(this WebApplication app)
    {
        // Uncomment the following line to enable the Prometheus endpoint (requires the OpenTelemetry.Exporter.Prometheus.AspNetCore package)
        // app.MapPrometheusScrapingEndpoint();

        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsEnvironment("local"))
        {
            // All health checks must pass for app to be considered ready to accept traffic after starting
            app.MapHealthChecks("/health")
                .ExcludeFromDescription()
                .WithHttpLogging(HttpLoggingFields.None);

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            }).ExcludeFromDescription().WithHttpLogging(HttpLoggingFields.None);
        }

        return app;
    }
}
