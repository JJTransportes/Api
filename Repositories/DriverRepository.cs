using Api.Data;
using Api.Dtos.Drivers;
using Api.Extensions.Drivers;
using Api.Models.Drivers;
using Api.Models.Info;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly AppDbContext _db;

    public DriverRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<DriverDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var drivers = await _db.Drivers.AsNoTracking().ToListAsync(cancellationToken);

        var driverIds = drivers.Select(d => d.Id).ToList();
        var phones = await _db.PhoneNumbers
            .AsNoTracking()
            .Where(p => driverIds.Contains(p.UserId) && p.UserType == Enums.UserType.Driver)
            .ToListAsync(cancellationToken);

        return drivers.Select(d => d.MapToDto(phones.Where(p => p.UserId == d.Id).ToList())).ToList();
    }

    public async Task<DriverDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var driver = await _db.Drivers.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (driver is null) return null;

        var phones = await _db.PhoneNumbers
            .AsNoTracking()
            .Where(p => p.UserId == id && p.UserType == Enums.UserType.Driver)
            .ToListAsync(cancellationToken);

        return driver.MapToDto(phones);
    }

    public async Task<DriverDto> CreateAsync(NewDriverDto dto, CancellationToken cancellationToken = default)
    {
        var driver = new Driver
        {
            Id = dto.UserId,
            FullName = dto.FullName,
            Cpf = dto.Cpf,
            Email = dto.Email,
            Birthdate = dto.Birthdate,
            Gender = dto.Gender,
            CreatedAt = DateTime.UtcNow,
            Approved = false
        };

        _db.Drivers.Add(driver);

        var phones = dto.Phones.Select(p => new PhoneNumber
        {
            Id = Guid.NewGuid(),
            UserId = driver.Id,
            UserType = Enums.UserType.Driver,
            Number = p.Number,
            AreaCode = p.AreaCode,
            CountryCode = p.CountryCode,
            Type = p.Type
        }).ToList();

        if (phones.Count > 0)
            _db.PhoneNumbers.AddRange(phones);

        await _db.SaveChangesAsync(cancellationToken);

        return driver.MapToDto(phones);
    }

    public async Task<DriverDto?> UpdateAsync(Guid id, UpdateDriverDto dto, CancellationToken cancellationToken = default)
    {
        var driver = await _db.Drivers.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (driver is null) return null;

        if (dto.FullName is not null) driver.FullName = dto.FullName;
        if (dto.Email is not null) driver.Email = dto.Email;
        if (dto.Birthdate is not null) driver.Birthdate = dto.Birthdate.Value;
        if (dto.Gender is not null) driver.Gender = dto.Gender;
        if (dto.Approved is not null) driver.Approved = dto.Approved.Value;

        if (dto.Phones is not null)
        {
            var existingPhones = await _db.PhoneNumbers
                .Where(p => p.UserId == id && p.UserType == Enums.UserType.Driver)
                .ToListAsync(cancellationToken);

            _db.PhoneNumbers.RemoveRange(existingPhones);

            var newPhones = dto.Phones.Select(p => new PhoneNumber
            {
                Id = Guid.NewGuid(),
                UserId = driver.Id,
                UserType = Enums.UserType.Driver,
                Number = p.Number,
                AreaCode = p.AreaCode,
                CountryCode = p.CountryCode,
                Type = p.Type
            }).ToList();

            if (newPhones.Count > 0)
                _db.PhoneNumbers.AddRange(newPhones);
        }

        await _db.SaveChangesAsync(cancellationToken);

        var phones = await _db.PhoneNumbers
            .AsNoTracking()
            .Where(p => p.UserId == id && p.UserType == Enums.UserType.Driver)
            .ToListAsync(cancellationToken);

        return driver.MapToDto(phones);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var driver = await _db.Drivers.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (driver is null) return false;

        var phones = await _db.PhoneNumbers
            .Where(p => p.UserId == id && p.UserType == Enums.UserType.Driver)
            .ToListAsync(cancellationToken);

        var address = await _db.Addresses
            .FirstOrDefaultAsync(a => a.UserId == id && a.UserType == Enums.UserType.Driver, cancellationToken);

        _db.PhoneNumbers.RemoveRange(phones);
        if (address is not null) _db.Addresses.Remove(address);
        _db.Drivers.Remove(driver);

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
