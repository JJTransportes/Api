using Api.Dtos.Customers;
using Api.Dtos.Info;
using Api.Models.Customers;
using Api.Models.Info;

namespace Api.Extensions.Customers;

public static class CustomerExtensions
{
    public static CustomerDto MapToDto(this Customer customer, List<PhoneNumber> phones)
    {
        return new CustomerDto(
            customer.Id,
            customer.FullName,
            customer.Cpf,
            customer.Email,
            customer.Birthdate,
            customer.CreatedAt,
            customer.Gender,
            customer.Approved,
            phones.Select(p => new PhoneDto(
                p.Id,
                p.Number,
                p.AreaCode,
                p.CountryCode,
                p.Type
            )).ToList()
        );
    }
}
