using Amazon.DynamoDBv2.DataModel;
using DynamoDb.Api.Data.Repositories.DynamoDb;
using DynamoDb.Api.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DynamoDb.Api.Data.Repositories.Postgres;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDynamoCustomerRepository _dynamoCustomerRepository;
    
    public CustomerRepository(ApplicationDbContext dbContext, IDynamoCustomerRepository dynamoCustomerRepository)
    {
        _dbContext = dbContext;
        _dynamoCustomerRepository = dynamoCustomerRepository;
    }
    
    public async Task<bool> AddCustomer(AddCustomerRequest request)
    {
        var customer = request.Adapt<Customer>();
        
        _dbContext.Customers.Add(customer);
        
        var result =  await _dbContext.SaveChangesAsync();

        if (result > 0)
        {
            await AddToDynamoDb(customer);
        }
        
        return result > 0;
    }
    
    public async Task<Customer?> GetCustomer(string id)
    {
        return await _dbContext.Customers.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<List<Customer>> GetCustomers()
    {
        return await _dbContext.Customers.Where(x => !x.IsDeleted).ToListAsync();
    }
    
    public async Task<int> SoftDeleteCustomer(string id)
    {
        var isDeleted = await _dbContext.Customers.Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsDeleted, true));
        
        if (isDeleted > 0)
            await HardDeleteFromDynamoDb(id);

        return isDeleted;
    }
    
    private async Task<bool> AddToDynamoDb(Customer customer)
    {
        await _dynamoCustomerRepository.SaveAsync(customer);
        
        return true;
    }
    
    private async Task<bool> HardDeleteFromDynamoDb(string customerId)
    {
        await _dynamoCustomerRepository.DeleteAsync(customerId);
        
        return true;
    }
    
    private async Task<bool> SoftDeleteFromDynamoDb(string customerId)
    {
        var customer = await _dynamoCustomerRepository.GetAsync(customerId);
        
        if (customer != null)
        {
            customer.IsDeleted = true;
            
            await _dynamoCustomerRepository.SaveAsync(customer);

            return true;
        }
        
        return false;
    }
}
