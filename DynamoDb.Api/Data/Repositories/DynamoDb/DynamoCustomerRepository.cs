using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDb.Api.Options;
using Microsoft.Extensions.Options;

namespace DynamoDb.Api.Data.Repositories.DynamoDb;

public class DynamoCustomerRepository : IDynamoCustomerRepository
{
    private readonly IDynamoDBContext _context;
    private readonly DynamoDBOperationConfig _tablesConfig; // from Amazon.DynamoDBv2.DataModel library
    
    public DynamoCustomerRepository(IDynamoDBContext context, IOptions<DynamoTablesConfig> options)
    {
        _context = context;
        _tablesConfig = new DynamoDBOperationConfig
        {
            OverrideTableName = options.Value.CustomerTableName
        };
    }
    
    public async Task SaveAsync(Customer customer)
    {
        await _context.SaveAsync(customer, _tablesConfig);
    }
    
    public async Task<Customer?> GetAsync(string customerId)
    {
       var result = await _context.LoadAsync<Customer?>(customerId, _tablesConfig);
       
         return result;
        
    }
    
    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        var conditions = new List<ScanCondition>
        {
            new ("Email", ScanOperator.Equal, email)
        };
        
        var search = _context.ScanAsync<Customer>(conditions, _tablesConfig);

        return await Task.FromResult(search.GetRemainingAsync().Result.FirstOrDefault());
    }
    
    public async Task<List<Customer>> GetCustomers()
    {
        var search = _context.ScanAsync<Customer>(new List<ScanCondition>(), _tablesConfig);
        
        return await Task.FromResult(search.GetRemainingAsync().Result);
    }
    
    public async Task<bool> DeleteAsync(string customerId)
    {
        var customer = await GetAsync(customerId);
        
        if (customer is null)
        {
            return false;
        }
        
        await _context.DeleteAsync(customer, _tablesConfig);
        
        return true;
    }
}
