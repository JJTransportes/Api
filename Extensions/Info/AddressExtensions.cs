using Api.Dtos.Info;
using Api.Models.Info;

namespace Api.Extensions.Info;

public static class AddressExtensions
{
    public static AddressDto MapToDto(this Address address)
    {
        return new AddressDto(
            address.Id,
            address.Street,
            address.Number,
            address.ZipCode,
            address.Neighborhood,
            address.City,
            address.State,
            address.Country,
            address.Complement,
            address.Line1,
            address.Line2
        );
    }
}
