namespace Tus.Api.Domain.Aggregates.HealthAggregate;

using Tus.Api.Domain.Common;

public interface IHealthRepository : IRepository<Health>
{
    ServerInfo GetServerInfo();
}
