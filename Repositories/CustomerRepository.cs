using Api.Data;
using Api.Dtos.Customers;
using Api.Extensions.Customers;
using Api.Models.Customers;
using Api.Models.Info;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _db.Customers.AsNoTracking().ToListAsync(cancellationToken);

        var customerIds = customers.Select(c => c.Id).ToList();
        var phones = await _db.PhoneNumbers
            .AsNoTracking()
            .Where(p => customerIds.Contains(p.UserId) && p.UserType == Enums.UserType.Customer)
            .ToListAsync(cancellationToken);

        return customers.Select(c => c.MapToDto(phones.Where(p => p.UserId == c.Id).ToList())).ToList();
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (customer is null) return null;

        var phones = await _db.PhoneNumbers
            .AsNoTracking()
            .Where(p => p.UserId == id && p.UserType == Enums.UserType.Customer)
            .ToListAsync(cancellationToken);

        return customer.MapToDto(phones);
    }

    public async Task<CustomerDto> CreateAsync(NewCustomerDto dto, CancellationToken cancellationToken = default)
    {
        var customer = new Customer
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

        _db.Customers.Add(customer);

        var phones = dto.Phones.Select(p => new PhoneNumber
        {
            Id = Guid.NewGuid(),
            UserId = customer.Id,
            UserType = Enums.UserType.Customer,
            Number = p.Number,
            AreaCode = p.AreaCode,
            CountryCode = p.CountryCode,
            Type = p.Type
        }).ToList();

        if (phones.Count > 0)
            _db.PhoneNumbers.AddRange(phones);

        await _db.SaveChangesAsync(cancellationToken);

        return customer.MapToDto(phones);
    }

    public async Task<CustomerDto?> UpdateAsync(Guid id, UpdateCustomerDto dto, CancellationToken cancellationToken = default)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (customer is null) return null;

        if (dto.FullName is not null) customer.FullName = dto.FullName;
        if (dto.Email is not null) customer.Email = dto.Email;
        if (dto.Birthdate is not null) customer.Birthdate = dto.Birthdate.Value;
        if (dto.Gender is not null) customer.Gender = dto.Gender;
        if (dto.Approved is not null) customer.Approved = dto.Approved.Value;

        if (dto.Phones is not null)
        {
            var existingPhones = await _db.PhoneNumbers
                .Where(p => p.UserId == id && p.UserType == Enums.UserType.Customer)
                .ToListAsync(cancellationToken);

            _db.PhoneNumbers.RemoveRange(existingPhones);

            var newPhones = dto.Phones.Select(p => new PhoneNumber
            {
                Id = Guid.NewGuid(),
                UserId = customer.Id,
                UserType = Enums.UserType.Customer,
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
            .Where(p => p.UserId == id && p.UserType == Enums.UserType.Customer)
            .ToListAsync(cancellationToken);

        return customer.MapToDto(phones);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (customer is null) return false;

        var phones = await _db.PhoneNumbers
            .Where(p => p.UserId == id && p.UserType == Enums.UserType.Customer)
            .ToListAsync(cancellationToken);

        var address = await _db.Addresses
            .FirstOrDefaultAsync(a => a.UserId == id && a.UserType == Enums.UserType.Customer, cancellationToken);

        _db.PhoneNumbers.RemoveRange(phones);
        if (address is not null) _db.Addresses.Remove(address);
        _db.Customers.Remove(customer);

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
