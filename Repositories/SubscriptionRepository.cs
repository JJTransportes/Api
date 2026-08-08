using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Api.Config;
using Api.Data;
using Api.Dtos.Subscriptions;
using Api.Enums;
using Api.Extensions.Subscriptions;
using Api.Models.Subscriptions;

namespace Api.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _db;
    private readonly AppConfig _config;

    public SubscriptionRepository(AppDbContext db, IOptions<AppConfig> config)
    {
        _db = db;
        _config = config.Value;
    }

    public async Task<List<SubscriptionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var subscriptions = await _db.Subscriptions
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return subscriptions.Select(s => s.MapToDto()).ToList();
    }

    public async Task<SubscriptionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var subscription = await _db.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return subscription?.MapToDto();
    }

    public async Task<SubscriptionDto?> GetByDriverIdAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        var subscription = await _db.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == driverId, cancellationToken);

        return subscription?.MapToDto();
    }

    public async Task<SubscriptionDto> CreateAsync(NewSubscriptionDto dto, CancellationToken cancellationToken = default)
    {
        var driver = await _db.Drivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == dto.UserId, cancellationToken);

        if (driver is null)
            throw new InvalidOperationException($"Motorista com id '{dto.UserId}' não encontrado.");

        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            TaxMode = dto.TaxMode,
            Status = SignatureStatus.Pending,
            SplitPercentage = dto.TaxMode == TaxMode.SingleTax ? 100 : 90,
            SubscriptionPrice = dto.TaxMode == TaxMode.SingleTax ? _config.SubscriptionPrice : 0
        };

        _db.Subscriptions.Add(subscription);
        await _db.SaveChangesAsync(cancellationToken);

        return subscription.MapToDto();
    }

    public async Task<SubscriptionDto?> UpdateAsync(Guid id, UpdateSubscriptionDto dto, CancellationToken cancellationToken = default)
    {
        var subscription = await _db.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (subscription is null) return null;

        if (dto.TaxMode is not null)
        {
            subscription.TaxMode = dto.TaxMode.Value;
            subscription.SplitPercentage = dto.TaxMode.Value == TaxMode.SingleTax ? 100 : 90;
            subscription.SubscriptionPrice = dto.TaxMode.Value == TaxMode.SingleTax ? _config.SubscriptionPrice : 0;
        }
        if (dto.Status is not null) subscription.Status = dto.Status.Value;
        if (dto.SplitPercentage is not null) subscription.SplitPercentage = dto.SplitPercentage.Value;
        if (dto.SubscriptionPrice is not null) subscription.SubscriptionPrice = dto.SubscriptionPrice.Value;

        await _db.SaveChangesAsync(cancellationToken);

        return subscription.MapToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var subscription = await _db.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (subscription is null) return false;

        _db.Subscriptions.Remove(subscription);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
