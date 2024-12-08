using ElasticSearch.Api.Dtos;
using ElasticSearch.Api.Entities;
using ElasticSearch.Api.Extensions;
using ElasticSearch.Api.Options;
using Mapster;
using Microsoft.Extensions.Options;
using Nest;

namespace ElasticSearch.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<CustomerService> _logger;
    
    public CustomerService(IElasticClient elasticClient,
    ILogger<CustomerService> logger)
    {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task<bool> AddAsync(AddCustomerRequest request)
    {
        _logger.LogInformation("Adding customer with payload: {Payload}", request.Serialize());
        
        var customer = request.Adapt<Customer>();
        
        var response = await _elasticClient.IndexDocumentAsync(customer);
        
        return response.IsValid;
    }

    public async Task<Customer?> GetAsync(string id)
    {
        _logger.LogInformation("Getting customer by id: {id}", id);
        
        var customer = await _elasticClient.GetAsync<Customer>(id);

        return customer.Source;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        _logger.LogInformation("Getting all customers");
        
        var searchResponse = await _elasticClient.SearchAsync<Customer>();
        
        return searchResponse.Documents.ToList();
    }
    
    public async Task<bool> UpdateAsync(UpdateCustomerRequest request)
    {
        _logger.LogInformation("Updating customer with and payload: {Payload}", request.Serialize());
        
        var customer = request.Adapt<Customer>();
        
        var response = await _elasticClient.UpdateAsync<Customer>(request.Id, u => u.Doc(customer));
        
        return response.IsValid;
    }
    
    public async Task<bool> DeleteAsync(string id)
    {
        _logger.LogInformation("Deleting customer with id: {id}", id);
        
        var response = await _elasticClient.DeleteAsync<Customer>(id);
        
        return response.IsValid;
    }
}
