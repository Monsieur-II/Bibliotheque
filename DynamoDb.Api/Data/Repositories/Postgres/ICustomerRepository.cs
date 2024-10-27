using DynamoDb.Api.Dtos;

namespace DynamoDb.Api.Data.Repositories.Postgres;

public interface ICustomerRepository
{
    Task<bool> AddCustomer(AddCustomerRequest request);
    Task<Customer?> GetCustomer(string id);
    Task<List<Customer>> GetCustomers();
    Task<int> SoftDeleteCustomer(string id);
}
