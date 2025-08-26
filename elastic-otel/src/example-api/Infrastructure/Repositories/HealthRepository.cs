namespace Example.Api.Infrastructure.Repositories;

using Example.Api.Domain.Aggregates.HealthAggregate;
using Example.Api.Domain.Common;

public class HealthRepository() : IHealthRepository
{
    public IUnitOfWork UnitOfWork => null;

    public ServerInfo GetServerInfo() => new();
}
