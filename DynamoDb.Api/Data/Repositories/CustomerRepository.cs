using Microsoft.EntityFrameworkCore;

namespace DynamoDb.Api.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public CustomerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> AddCustomer(Customer customer)
    {
        _dbContext.Customers.Add(customer);
        
        return await _dbContext.SaveChangesAsync();
    }
    
    public async Task<Customer?> GetCustomer(string id)
    {
        return await _dbContext.Customers.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<List<Customer>> GetCustomers()
    {
        return await _dbContext.Customers.Where(x => !x.IsDeleted).ToListAsync();
    }
    
    public async Task<int> DeleteCustomer(string id)
    {
        var isDeleted = await _dbContext.Customers.Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsDeleted, true));

        return isDeleted;
    }
}
