using Employees.Data.Entities;
using Employees.Infrastructure.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.Data.Configuration;

internal class SystemLogConfiguration : BaseConfiguration<SystemLogEntity>
{
    private readonly IJsonSerializer _serializer;
    public SystemLogConfiguration(IJsonSerializer jsonSerializer)
    {
        _serializer = jsonSerializer;
    }

    public override void Configure(EntityTypeBuilder<SystemLogEntity> builder)
    {
        base.Configure(builder);

        builder
            .Property(x => x.ResourceType)
            .HasConversion<string>();

        builder
            .Property(x => x.Event)
            .HasConversion<string>();

        builder.Property(e => e.ChangeSet)
            .HasConversion(
                v => _serializer.Serialize(v),
                v => _serializer.Deserialize<Dictionary<string, object>>(v!)!
            );
    }
}
