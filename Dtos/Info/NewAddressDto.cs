namespace Api.Dtos.Info;

public record NewAddressDto(
    string Street,
    string Number,
    string ZipCode,
    string Neighborhood,
    string City,
    string State,
    string Country,
    string? Complement,
    string? Line1,
    string? Line2
);
