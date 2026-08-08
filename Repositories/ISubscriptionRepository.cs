using Api.Dtos.Subscriptions;

namespace Api.Repositories;

public interface ISubscriptionRepository
{
    Task<List<SubscriptionDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SubscriptionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SubscriptionDto?> GetByDriverIdAsync(Guid driverId, CancellationToken cancellationToken = default);
    Task<SubscriptionDto> CreateAsync(NewSubscriptionDto dto, CancellationToken cancellationToken = default);
    Task<SubscriptionDto?> UpdateAsync(Guid id, UpdateSubscriptionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
