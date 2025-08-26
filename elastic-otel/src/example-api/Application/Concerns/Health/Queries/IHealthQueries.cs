namespace Example.Api.Application.Concerns.Health.Queries;

using Example.Api.Application.Concerns.Health.Models;

public interface IHealthQueries
{
    PingRs GetPing();

    VersionRs GetVersion();
}
