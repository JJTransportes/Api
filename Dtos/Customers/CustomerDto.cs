using Api.Dtos.Info;

namespace Api.Dtos.Customers;

public record CustomerDto(
    Guid Id,
    string FullName,
    string Cpf,
    string Email,
    DateTime Birthdate,
    DateTime CreatedAt,
    string Gender,
    bool Approved,
    List<PhoneDto> Phones
);
