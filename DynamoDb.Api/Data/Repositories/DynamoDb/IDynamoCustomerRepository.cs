namespace DynamoDb.Api.Data.Repositories.DynamoDb;

public interface IDynamoCustomerRepository
{
    Task SaveAsync(Customer customer);
    Task<Customer?> GetAsync(string customerId);
    Task<List<Customer>> GetCustomers();
    Task<Customer?> GetCustomerByEmailAsync(string email);
    Task<bool> DeleteAsync(string customerId);
}
