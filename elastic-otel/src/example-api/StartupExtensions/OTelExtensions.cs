namespace Example.Api.StartupExtensions;

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

internal static class OTelExtensions
{
    private readonly static string[] ExcludedOTelRoutes = ["/metrics", "/alive", "/health"];

    internal static IHostApplicationBuilder AddApplicationOpenTelemetry(this IHostApplicationBuilder builder, string serviceName)
    {
        builder.Services.AddLogging();

        Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .AddAspNetCoreInstrumentation((o) => o.Filter = (httpContext) =>
            {
                if (ExcludedOTelRoutes.Contains(httpContext.Request.Path.Value, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }

                return true;
            })
            .AddHttpClientInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                options.ExportProcessorType = ExportProcessorType.Simple;
                options.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://otel-collector:4317");
            })
            .Build();

        Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .AddRuntimeInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                options.ExportProcessorType = ExportProcessorType.Simple;
                options.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://otel-collector:4317");
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
                        options.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://otel-collector:4317");
                    })
                    .AddConsoleExporter();
            });
        });

        return builder;
    }
}
