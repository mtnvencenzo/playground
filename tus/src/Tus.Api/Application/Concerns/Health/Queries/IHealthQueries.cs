namespace Tus.Api.Application.Concerns.Health.Queries;

using global::Tus.Api.Application.Concerns.Health.Models;

public interface IHealthQueries
{
    PingRs GetPing();

    VersionRs GetVersion();
}
