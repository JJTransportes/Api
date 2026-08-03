using Api.Dtos.Info;

namespace Api.Repositories;

public interface IAddressRepository
{
    Task<AddressDto?> GetByUserAsync(Guid userId, Enums.UserType userType, CancellationToken cancellationToken = default);
    Task<AddressDto> CreateAsync(Guid userId, Enums.UserType userType, NewAddressDto dto, CancellationToken cancellationToken = default);
    Task<AddressDto> UpdateAsync(Guid userId, Enums.UserType userType, UpdateAddressDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid userId, Enums.UserType userType, CancellationToken cancellationToken = default);
}
