using Api.Dtos.Admins;

namespace Api.Repositories;

public interface IAdminRepository
{
    Task<List<AdminDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AdminDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AdminDto> CreateAsync(NewAdminDto dto, CancellationToken cancellationToken = default);
    Task<AdminDto?> UpdateAsync(Guid id, UpdateAdminDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
