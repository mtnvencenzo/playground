namespace Tus.Api.StartupExtensions;

using Tus.Api.Application.Behaviors.MediatRPipelines;
using Tus.Api.Application.Concerns.Health.Queries;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tus.Api.Domain.Config;
using Tus.Api.Domain.Aggregates.TusUploadAggregate;
using Tus.Api.Domain.Aggregates.HealthAggregate;
using Tus.Api.Infrastructure.Repositories;

internal static class ApplicationServiceExtensions
{
    internal static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        var env = builder.Environment.EnvironmentName;

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json.user", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        builder.Services.Configure<TusApiConfig>(builder.Configuration.GetSection(TusApiConfig.SectionName));

        // Register the dapr client
        builder.Services.AddDaprClient();

        // Add dapr serice bus messaging to DI
        builder.Services.AddEventBus(builder.Configuration);

        // Add dapr blob storage to DI
        builder.Services.AddStorageBus();

        // Add mediator and commands to DI
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });

        // Add queries to DI
        builder.Services.AddScoped<IHealthQueries, HealthQueries>();

        // Add validators for the MediatR validation pipeline behavior (validators based on FluentValidation library)
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        // Add repositories to DI
        builder.Services.AddScoped<ITusRepository, TusRepository>();
        builder.Services.AddScoped<IHealthRepository, HealthRepository>();
    }
}