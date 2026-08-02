using Api.Dtos.Customers;

namespace Api.Repositories;

public interface ICustomerRepository
{
    Task<List<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CustomerDto> CreateAsync(NewCustomerDto dto, CancellationToken cancellationToken = default);
    Task<CustomerDto?> UpdateAsync(Guid id, UpdateCustomerDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
