using Api.Dtos.Info;

namespace Api.Dtos.Admins;

public record AdminDto(
    Guid Id,
    string FullName,
    string Cpf,
    string Email,
    DateTime Birthdate,
    DateTime CreatedAt,
    bool Approved,
    List<PhoneDto> Phones
);
