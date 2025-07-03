namespace Tus.Api.StartupExtensions;

using Asp.Versioning;
using Tus.Api.Apis.TusUpload;
using Tus.Api.Domain.Config;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tus.Api.StartupExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

internal static class OpenApiExtensions
{
    internal static IHostApplicationBuilder AddDefaultOpenApi(
        this IHostApplicationBuilder builder,
        IApiVersioningBuilder apiVersioning = default)
    {
        if (apiVersioning is not null)
        {
            // the default format will just be ApiVersion.ToString(); for example, 1.0.
            // this will format the version as "'v'major[.minor][-status]"
            var versioned = apiVersioning.AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            string[] versions = ["v1"];

            foreach (var version in versions)
            {
                builder.Services.AddOpenApi(version, options =>
                {
                    options.ApplyApiVersionInfo("Tus Api", "Tus Api", "/api-docs-logo.png", "Api documentation for the tus api");
                    options.ApplyAuthorizationChecks();
                    options.ApplySecuritySchemeDefinitions();
                    options.ApplyOperationDeprecatedStatus();
                    options.ApplyApiVersionDescription();
                    options.ApplySchemaNullableFalse();
                    options.ApplySchemaPropertyExamples();
                    options.AddOperationSubscriptionKeyHeader();
                });
            }
        }

        return builder;
    }

    internal static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        var tusApiOptions = app.Services.GetRequiredService<IOptions<TusApiConfig>>();

        var openApiPattern = "/scalar/{documentName}/openapi.json";

        app.MapOpenApi(pattern: openApiPattern);

        app.MapScalarApiReference(options =>
        {
            // Disable default fonts to avoid download unnecessary fonts
            options.DefaultFonts = false;
            options.Favicon = "/favicon.svg";
            options.Title = "Cezzi's Tus Api";
            options.Theme = ScalarTheme.Purple;
            options.OpenApiRoutePattern = openApiPattern;
            options.Servers =
            [
                new ScalarServer(tusApiOptions.Value.BaseOpenApiUri, "Default")
            ];
        });

        app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();

        return app;
    }
}
