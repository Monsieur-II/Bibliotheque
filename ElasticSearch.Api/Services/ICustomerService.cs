using ElasticSearch.Api.Dtos;
using ElasticSearch.Api.Entities;

namespace ElasticSearch.Api.Services;

public interface ICustomerService
{
    Task<Customer?> GetAsync(string id);
    Task<List<Customer>> GetAllAsync(BaseFilter filter);
    Task<bool> AddAsync(AddCustomerRequest request);
    Task<bool> UpdateAsync(UpdateCustomerRequest request);
    Task<bool> DeleteAsync(string id);
    Task<List<Customer>> SearchAsync(BaseFilter filter);
}
