namespace Tus.Api.Domain.Aggregates.TusUploadAggregate;

using Tus.Api.Domain.Common;

public interface ITusRepository : IRepository<TusUpload>, IReadonlyRepository<TusUpload>
{
}
