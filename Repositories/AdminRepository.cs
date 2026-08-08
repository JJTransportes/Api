using Api.Data;
using Api.Dtos.Admins;
using Api.Extensions.Admins;
using Api.Models.Admins;
using Api.Models.Info;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly AppDbContext _db;

    public AdminRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<AdminDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var admins = await _db.Admins.AsNoTracking().ToListAsync(cancellationToken);

        var adminIds = admins.Select(a => a.Id).ToList();
        var phones = await _db.PhoneNumbers
            .AsNoTracking()
            .Where(p => adminIds.Contains(p.UserId) && p.UserType == Enums.UserType.Admin)
            .ToListAsync(cancellationToken);

        return admins.Select(a => a.MapToDto(phones.Where(p => p.UserId == a.Id).ToList())).ToList();
    }

    public async Task<AdminDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var admin = await _db.Admins.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (admin is null) return null;

        var phones = await _db.PhoneNumbers
            .AsNoTracking()
            .Where(p => p.UserId == id && p.UserType == Enums.UserType.Admin)
            .ToListAsync(cancellationToken);

        return admin.MapToDto(phones);
    }

    public async Task<AdminDto> CreateAsync(NewAdminDto dto, CancellationToken cancellationToken = default)
    {
        var admin = new Admin
        {
            Id = dto.UserId,
            FullName = dto.FullName,
            Cpf = dto.Cpf,
            Email = dto.Email,
            Birthdate = dto.Birthdate,
            CreatedAt = DateTime.UtcNow,
            Approved = false
        };

        _db.Admins.Add(admin);

        var phones = dto.Phones.Select(p => new PhoneNumber
        {
            Id = Guid.NewGuid(),
            UserId = admin.Id,
            UserType = Enums.UserType.Admin,
            Number = p.Number,
            AreaCode = p.AreaCode,
            CountryCode = p.CountryCode,
            Type = p.Type
        }).ToList();

        if (phones.Count > 0)
            _db.PhoneNumbers.AddRange(phones);

        await _db.SaveChangesAsync(cancellationToken);

        return admin.MapToDto(phones);
    }

    public async Task<AdminDto?> UpdateAsync(Guid id, UpdateAdminDto dto, CancellationToken cancellationToken = default)
    {
        var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (admin is null) return null;

        if (dto.FullName is not null) admin.FullName = dto.FullName;
        if (dto.Email is not null) admin.Email = dto.Email;
        if (dto.Birthdate is not null) admin.Birthdate = dto.Birthdate.Value;
        if (dto.Approved is not null) admin.Approved = dto.Approved.Value;

        if (dto.Phones is not null)
        {
            var existingPhones = await _db.PhoneNumbers
                .Where(p => p.UserId == id && p.UserType == Enums.UserType.Admin)
                .ToListAsync(cancellationToken);

            _db.PhoneNumbers.RemoveRange(existingPhones);

            var newPhones = dto.Phones.Select(p => new PhoneNumber
            {
                Id = Guid.NewGuid(),
                UserId = admin.Id,
                UserType = Enums.UserType.Admin,
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
            .Where(p => p.UserId == id && p.UserType == Enums.UserType.Admin)
            .ToListAsync(cancellationToken);

        return admin.MapToDto(phones);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        if (admin is null) return false;

        var phones = await _db.PhoneNumbers
            .Where(p => p.UserId == id && p.UserType == Enums.UserType.Admin)
            .ToListAsync(cancellationToken);

        _db.PhoneNumbers.RemoveRange(phones);
        _db.Admins.Remove(admin);

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
