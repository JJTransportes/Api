namespace Api.Dtos.Info;

public record PhoneDto(
    Guid Id,
    string Number,
    string AreaCode,
    string CountryCode,
    string Type
);
