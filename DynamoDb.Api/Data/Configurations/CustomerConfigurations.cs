using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamoDb.Api.Data.Configurations;

public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasData(
            new Customer { Id = Guid.NewGuid().ToString("N"), Name = "John Doe" },
            new Customer { Id = Guid.NewGuid().ToString("N"), Name = "Jane Doe" });
    }
}
