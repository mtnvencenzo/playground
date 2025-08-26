namespace Example.Api.Apis.Health;

using Example.Api.Application.Concerns.Health.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Net;

/// <summary>
/// 
/// </summary>
public static class HealthApi
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static RouteGroupBuilder MapHealthApiV1(this IEndpointRouteBuilder app)
    {
        var groupBuilder = app.MapGroup("/health")
            .WithName(nameof(GetPing))
            .AllowAnonymous();

        groupBuilder.MapGet("/ping", GetPing)
            .WithName(nameof(GetPing))
            .WithDisplayName(nameof(GetPing));

        groupBuilder.MapGet("/version", GetVersion)
            .WithName(nameof(GetVersion))
            .WithDisplayName(nameof(GetVersion));

        return groupBuilder;
    }

    /// <summary>Pings the example api</summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PingRs))]
    [ProducesDefaultResponseType(typeof(ProblemDetails))]
    public static Ok<PingRs> GetPing([AsParameters] HealthServices healthServices)
    {
        healthServices.Logger.LogInformation("Getting the server information");

        var ping = healthServices.Queries.GetPing();

        return TypedResults.Ok(ping);
    }

    /// <summary>Gets the current example api version</summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(VersionRs))]
    [ProducesDefaultResponseType(typeof(ProblemDetails))]
    public static Ok<VersionRs> GetVersion([AsParameters] HealthServices healthServices)
    {
        healthServices.Logger.LogInformation("Getting the api version");

        var version = healthServices.Queries.GetVersion();

        return TypedResults.Ok(version);
    }
}