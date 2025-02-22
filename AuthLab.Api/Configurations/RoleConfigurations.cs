using AuthLab.Api.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthLab.Api.Configurations;

public class RoleConfigurations : BaseConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasMaxLength(10).IsRequired();
    }
}
