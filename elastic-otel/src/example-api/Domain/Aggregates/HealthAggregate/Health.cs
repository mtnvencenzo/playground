namespace Example.Api.Domain.Aggregates.HealthAggregate;

using Example.Api.Domain.Common;
using Example.Api.Domain.Exceptions;
using System;
using System.Text.Json.Serialization;

public class Health : Entity, IAggregateRoot
{
    [JsonInclude]
    public string SubjectId { get; private set; }

    [JsonInclude]
    public string Status { get; private set; }

    [JsonInclude]
    public string ETag { get; private set; }

    [JsonInclude]
    public string Discriminator { get; private set; }

    [JsonConstructor]
    protected Health() { }

    public Health(string subjectId, string status)
    {
        this.CreatedOn = DateTimeOffset.Now;
        this.UpdatedOn = DateTimeOffset.Now;
        this.SetSubjectId(subjectId);
        this.SetStatus(status);
    }

    public Health SetSubjectId(string subjectId)
    {
        if (string.IsNullOrWhiteSpace(subjectId))
        {
            throw new ExampleApiDomainException($"{nameof(subjectId)} not specified");
        }

        this.SubjectId = subjectId;
        this.UpdatedOn = DateTime.Now;

        return this;
    }

    public Health SetStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ExampleApiDomainException($"{nameof(status)} not specified");
        }

        this.Status = status;
        this.UpdatedOn = DateTime.Now;

        return this;
    }
}
