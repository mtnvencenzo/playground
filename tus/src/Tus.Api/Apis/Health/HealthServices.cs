namespace Tus.Api.Apis.Health;

using System;
using Tus.Api.Application.Concerns.Health.Queries;

/// <summary>
/// 
/// </summary>
/// <param name="queries"></param>
public class HealthServices(IHealthQueries queries)
{
    /// <summary></summary>
    public IHealthQueries Queries { get; } = queries ?? throw new ArgumentNullException(nameof(queries));
}