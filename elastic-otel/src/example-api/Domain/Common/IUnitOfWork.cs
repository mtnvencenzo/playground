namespace Example.Api.Domain.Common;

using System;
using System.Threading;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
