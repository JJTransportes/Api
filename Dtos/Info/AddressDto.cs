namespace Api.Dtos.Info;

public record AddressDto(
    Guid Id,
    string Street,
    string Number,
    string ZipCode,
    string Neighborhood,
    string City,
    string State,
    string Country,
    string Complement,
    string Line1,
    string Line2
);
