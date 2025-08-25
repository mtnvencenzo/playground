namespace Example.Api.StartupExtensions;

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
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

        Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                options.ExportProcessorType = ExportProcessorType.Simple;
                options.Endpoint = new Uri("http://otel-collector:4317");
            })
            .Build();

        Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                options.ExportProcessorType = ExportProcessorType.Simple;
                options.Endpoint = new Uri("http://otel-collector:4317");
            })
            .Build();

        Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                options.ExportProcessorType = ExportProcessorType.Simple;
                options.Endpoint = new Uri("http://otel-collector:4317");
            })
            .Build();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry(logging =>
            {
                logging
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                    .AddOtlpExporter(options =>
                    {
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        options.ExportProcessorType = ExportProcessorType.Simple;
                        options.Endpoint = new Uri("http://otel-collector:4317");
                    })
                    .AddConsoleExporter();
            });
        });

        // var openTelemetryBuilder = builder.Services
        //     .AddOpenTelemetry()
        //     .ConfigureResource(resource => resource.AddService(serviceName))
        //     .WithLogging(logging =>
        //     {
        //         logging
        //             .AddConsoleExporter()
        //             .AddOtlpExporter(options =>
        //             {
        //                 options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        //                 options.ExportProcessorType = ExportProcessorType.Simple;
        //                 options.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://otel-collector:4317");
        //             });
        //     })
        //     .WithTracing(tracing =>
        //     {
        //         tracing
        //             .SetSampler(new AlwaysOnSampler())
        //             .AddAspNetCoreInstrumentation()
        //             .AddHttpClientInstrumentation()
        //             .AddOtlpExporter(options =>
        //             {
        //                 options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        //                 options.ExportProcessorType = ExportProcessorType.Simple;
        //                 options.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://otel-collector:4317");
        //             });
        //     })
        //     .WithMetrics(metrics =>
        //     {
        //         metrics
        //             .AddAspNetCoreInstrumentation()
        //             .AddHttpClientInstrumentation()
        //             .AddOtlpExporter(options =>
        //             {
        //                 options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        //                 options.ExportProcessorType = ExportProcessorType.Simple;
        //                 options.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://otel-collector:4317");
        //             });
        //     });

        //openTelemetryBuilder.UseOtlpExporter( OpenTelemetry.Exporter.OtlpExportProtocol.Grpc, "")

        // if (!builder.Environment.IsEnvironment("local") && !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING")))
        // {
        //     openTelemetryBuilder.UseAzureMonitor();
        // }

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
