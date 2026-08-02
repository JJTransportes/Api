using Api.Dtos.Drivers;
using Api.Dtos.Info;
using Api.Models.Drivers;
using Api.Models.Info;

namespace Api.Extensions.Drivers;

public static class DriverExtensions
{
    public static DriverDto MapToDto(this Driver driver, List<PhoneNumber> phones)
    {
        return new DriverDto(
            driver.Id,
            driver.FullName,
            driver.Cpf,
            driver.Email,
            driver.Birthdate,
            driver.CreatedAt,
            driver.Gender,
            driver.Approved,
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
