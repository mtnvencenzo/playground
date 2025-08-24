namespace Example.Api.Infrastructure.EntityConfigurations;

using Example.Api.Domain.Aggregates.HealthAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class HealthEntityTypeConfiguration : IEntityTypeConfiguration<Health>, IHealthContextEntityConfiguration
{
    public void Configure(EntityTypeBuilder<Health> builder)
    {
        builder
            .ToContainer("health")
            .HasPartitionKey(x => x.SubjectId)
            .ApplyCamelCasingNamingStrategry()
            .HasKey(x => x.Id);

        builder.Property(x => x.ETag).IsETagConcurrency();

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.HasDiscriminator(x => x.Discriminator).HasValue("health");

        builder.Ignore(x => x.DomainEvents);
    }
}
