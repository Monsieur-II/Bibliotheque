using Microsoft.EntityFrameworkCore;

namespace DynamoDb.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Customer> Customers { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = Guid.NewGuid().ToString("N"), Name = "John Doe" },
            new Customer { Id = Guid.NewGuid().ToString("N"), Name = "Jane Doe" });
    }
}
