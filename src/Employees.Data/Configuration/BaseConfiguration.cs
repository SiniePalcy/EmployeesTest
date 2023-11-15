using Employees.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.Data.Configuration;

internal class BaseConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity, new()
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .HasQueryFilter(t => !t.IsDeleted);
    }
}
