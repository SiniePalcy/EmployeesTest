using Employees.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.Data.Configuration;

internal class EmployeeConfiguration : BaseConfiguration<EmployeeEntity>
{
    public override void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Title)
            .HasConversion<string>();

        builder.Property(x => x.Email)
            .HasConversion(
                x => x.Trim().ToLower(),
                x => x.Trim().ToLower());

        builder
            .HasMany(x => x.Companies)
            .WithMany(f => f.Employees)
            .UsingEntity(
                "CompanyEmployee",
                 l => l.HasOne(typeof(CompanyEntity)).WithMany().HasForeignKey("CompanyEntityId").HasPrincipalKey(nameof(CompanyEntity.Id)),
                 r => r.HasOne(typeof(EmployeeEntity)).WithMany().HasForeignKey("EmployeeEntityId").HasPrincipalKey(nameof(EmployeeEntity.Id)),
                 b => b.HasKey("CompanyEntityId", "EmployeeEntityId"));
    }
}
