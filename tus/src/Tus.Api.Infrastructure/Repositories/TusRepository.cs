namespace Tus.Api.Infrastructure.Repositories;

using System.Linq;
using Tus.Api.Domain.Aggregates.TusUploadAggregate;
using Tus.Api.Domain.Common;

public class TusRepository() : ITusRepository
{
    public IUnitOfWork UnitOfWork => null;

    public IQueryable<TusUpload> Items => throw new NotImplementedException();
}