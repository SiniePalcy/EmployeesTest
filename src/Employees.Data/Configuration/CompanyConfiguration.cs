using Employees.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Employees.Data.Configuration;

internal class CompanyConfiguration : BaseConfiguration<CompanyEntity>
{
    public override void Configure(EntityTypeBuilder<CompanyEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .HasConversion(
                x => x.Trim().ToUpper(),
                x => x.Trim().ToUpper());

    }
}
