using Api.Dtos.Info;

namespace Api.Dtos.Admins;

public record UpdateAdminDto(
    string? FullName,
    string? Email,
    DateTime? Birthdate,
    bool? Approved,
    List<NewPhoneDto>? Phones
);
