using AuthLab.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthLab.Api.Configurations;

public class StaffConfigurations : BaseConfiguration<Staff>
{
        public override void Configure(EntityTypeBuilder<Staff> builder)
        {
                base.Configure(builder);

                builder.Property(x => x.Permissions).HasColumnType("jsonb");
        }
}
