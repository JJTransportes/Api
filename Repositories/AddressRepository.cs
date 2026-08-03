using Api.Data;
using Api.Dtos.Info;
using Api.Extensions.Info;
using Api.Models.Info;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _db;

    public AddressRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AddressDto?> GetByUserAsync(Guid userId, Enums.UserType userType, CancellationToken cancellationToken = default)
    {
        var exists = await UserExistsAsync(userId, userType, cancellationToken);
        if (!exists) return null;

        var address = await _db.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.UserId == userId && a.UserType == userType, cancellationToken);

        return address?.MapToDto();
    }

    public async Task<AddressDto> CreateAsync(Guid userId, Enums.UserType userType, NewAddressDto dto, CancellationToken cancellationToken = default)
    {
        var exists = await UserExistsAsync(userId, userType, cancellationToken);
        if (!exists) throw new InvalidOperationException($"{userType} with id '{userId}' not found.");

        var address = new Address
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UserType = userType,
            Street = dto.Street,
            Number = dto.Number,
            ZipCode = dto.ZipCode,
            Neighborhood = dto.Neighborhood,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            Complement = dto.Complement ?? string.Empty,
            Line1 = dto.Line1 ?? string.Empty,
            Line2 = dto.Line2 ?? string.Empty
        };

        _db.Addresses.Add(address);
        await _db.SaveChangesAsync(cancellationToken);

        return address.MapToDto();
    }

    public async Task<AddressDto> UpdateAsync(Guid userId, Enums.UserType userType, UpdateAddressDto dto, CancellationToken cancellationToken = default)
    {
        var exists = await UserExistsAsync(userId, userType, cancellationToken);
        if (!exists) throw new InvalidOperationException($"{userType} with id '{userId}' not found.");

        var address = await _db.Addresses
            .FirstOrDefaultAsync(a => a.UserId == userId && a.UserType == userType, cancellationToken);

        if (address is null)
        {
            address = new Address
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                UserType = userType
            };
            _db.Addresses.Add(address);
        }

        if (dto.Street is not null) address.Street = dto.Street;
        if (dto.Number is not null) address.Number = dto.Number;
        if (dto.ZipCode is not null) address.ZipCode = dto.ZipCode;
        if (dto.Neighborhood is not null) address.Neighborhood = dto.Neighborhood;
        if (dto.City is not null) address.City = dto.City;
        if (dto.State is not null) address.State = dto.State;
        if (dto.Country is not null) address.Country = dto.Country;
        if (dto.Complement is not null) address.Complement = dto.Complement;
        if (dto.Line1 is not null) address.Line1 = dto.Line1;
        if (dto.Line2 is not null) address.Line2 = dto.Line2;

        await _db.SaveChangesAsync(cancellationToken);

        return address.MapToDto();
    }

    public async Task<bool> DeleteAsync(Guid userId, Enums.UserType userType, CancellationToken cancellationToken = default)
    {
        var address = await _db.Addresses
            .FirstOrDefaultAsync(a => a.UserId == userId && a.UserType == userType, cancellationToken);

        if (address is null) return false;

        _db.Addresses.Remove(address);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<bool> UserExistsAsync(Guid userId, Enums.UserType userType, CancellationToken cancellationToken)
    {
        return userType switch
        {
            Enums.UserType.Driver => await _db.Drivers.AnyAsync(d => d.Id == userId, cancellationToken),
            Enums.UserType.Customer => await _db.Customers.AnyAsync(c => c.Id == userId, cancellationToken),
            _ => false
        };
    }
}
