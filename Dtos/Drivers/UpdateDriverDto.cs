using Api.Dtos.Info;

namespace Api.Dtos.Drivers;

public record UpdateDriverDto(
    string? FullName,
    string? Email,
    DateTime? Birthdate,
    string? Gender,
    bool? Approved,
    List<NewPhoneDto>? Phones
);
