namespace DynamoDb.Api.Data.Repositories;

public interface ICustomerRepository
{
    Task<List<Customer>> GetCustomers();
}
