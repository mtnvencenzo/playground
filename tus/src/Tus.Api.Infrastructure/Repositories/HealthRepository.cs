namespace Tus.Api.Infrastructure.Repositories;

using Tus.Api.Domain.Aggregates.HealthAggregate;
using Tus.Api.Domain.Common;

public class HealthRepository() : IHealthRepository
{
    public IUnitOfWork UnitOfWork => null;

    public ServerInfo GetServerInfo() => new();
}
