namespace Example.Api.Domain.Aggregates.HealthAggregate;

using Example.Api.Domain.Common;

public interface IHealthRepository : IRepository<Health>
{
    ServerInfo GetServerInfo();
}
