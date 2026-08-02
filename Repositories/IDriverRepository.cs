using Api.Dtos.Drivers;

namespace Api.Repositories;

public interface IDriverRepository
{
    Task<List<DriverDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DriverDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DriverDto> CreateAsync(NewDriverDto dto, CancellationToken cancellationToken = default);
    Task<DriverDto?> UpdateAsync(Guid id, UpdateDriverDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
