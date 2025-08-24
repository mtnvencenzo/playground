namespace Example.Api.StartupExtensions;

using System;
using System.Linq;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

internal static class ServiceDefaultsExtensions
{
    private readonly static string[] ExcludedOTelRoutes = ["/metrics", "/alive", "/health"];

    internal static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.AddBasicServiceDefaults("example-api");

        builder.Services.AddHttpCors(builder.Configuration);

        builder.Services.ConfigureJsonSerialization();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.AddServiceDiscovery();
        });

        builder.Services.AddProblemDetails();

        return builder;
    }

    private static IHostApplicationBuilder AddBasicServiceDefaults(this IHostApplicationBuilder builder, string serviceName)
    {
        builder.Services.AddOptions();
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();

        // TODO: Is this needed
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        // Default health checks assume the event bus and self health checks
        builder.AddDefaultHealthChecks();

        builder.AddApplicationOpenTelemetry(serviceName);

        return builder;
    }

    private static IHostApplicationBuilder AddApplicationOpenTelemetry(this IHostApplicationBuilder builder, string serviceName)
    {
        builder.Services.AddLogging();

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName));

            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.AddOtlpExporter();
            if (builder.Environment.IsEnvironment("local"))
            {
                options.AddConsoleExporter();
            }
        });

        var openTelemetryBuilder = builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing =>
            {
                tracing.AddOtlpExporter();

                if (builder.Environment.IsEnvironment("local"))
                {
                    tracing.SetSampler(new AlwaysOnSampler());
                    tracing.AddConsoleExporter();
                }

                tracing
                    .AddAspNetCoreInstrumentation((o) => o.Filter = (httpContext) =>
                    {
                        if (ExcludedOTelRoutes.Contains(httpContext.Request.Path.Value, StringComparer.OrdinalIgnoreCase))
                        {
                            return false;
                        }

                        return true;
                    })
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation();
            })
            .WithMetrics(metrics =>
            {
                metrics.AddOtlpExporter();
                metrics.AddAspNetCoreInstrumentation();

                if (builder.Environment.IsEnvironment("local"))
                {
                    metrics.AddConsoleExporter();
                }
            });

        if (!builder.Environment.IsEnvironment("local") && !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING")))
        {
            openTelemetryBuilder.UseAzureMonitor();
        }

        return builder;
    }

    private static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        // Add a default liveness check to ensure app is responsive
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }
}
