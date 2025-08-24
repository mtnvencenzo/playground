namespace Example.Api.StartupExtensions;

using Example.Api.Application.Behaviors.MediatRPipelines;
using Example.Api.Application.Concerns.Health.Queries;
using Example.Api.Domain.Aggregates.HealthAggregate;
using Example.Api.Domain.Config;
using Example.Api.Infrastructure.Repositories;
using Example.Api.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        builder.Services.Configure<ExampleApiConfig>(builder.Configuration.GetSection(ExampleApiConfig.SectionName));
        builder.Services.Configure<CosmosDbConfig>(builder.Configuration.GetSection(CosmosDbConfig.SectionName));

        builder.Services.AddCosomsContexts();

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
        builder.Services.AddScoped<IHealthRepository, HealthRepository>();

        // add in infrastructure services
        builder.Services.AddTransient<IRequestHeaderAccessor, RequestHeaderAccessor>();
    }
}