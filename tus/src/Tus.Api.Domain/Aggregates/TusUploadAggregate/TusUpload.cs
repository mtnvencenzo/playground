namespace Tus.Api.Domain.Aggregates.TusUploadAggregate;

using Cezzi.Applications.Text;
using Tus.Api.Domain.Common;
using Tus.Api.Domain.Exceptions;
using System.Text.Json.Serialization;

public class TusUpload : Entity, IAggregateRoot
{
    [JsonConstructor]
    protected TusUpload()
    {
    }

    public TusUpload(string id)
        : this()
    {
        this.Id = id;
    }

    [JsonInclude]
    public string Hash { get; private set; }

    private TusUpload SetHash(string hash)
    {
        this.Hash = !string.IsNullOrWhiteSpace(hash)
            ? hash
            : throw new TusApiDomainException($"{nameof(hash)} cannot be null or empty");

        return this;
    }

    public string RegenerateHash()
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(this.Id);

        this.Hash = Base64.Encode(bytes).GetHashCode().ToString();
        return this.Hash;
    }
}