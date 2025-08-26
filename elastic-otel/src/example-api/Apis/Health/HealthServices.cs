namespace Example.Api.Apis.Health;

using System;
using Example.Api.Application.Concerns.Health.Queries;
using Microsoft.Extensions.Logging;

/// <summary>
/// 
/// </summary>
/// <param name="queries"></param>
public class HealthServices(IHealthQueries queries, ILogger<HealthServices> logger)
{
    /// <summary></summary>
    public IHealthQueries Queries { get; } = queries ?? throw new ArgumentNullException(nameof(queries));

    public ILogger<HealthServices> Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));
}