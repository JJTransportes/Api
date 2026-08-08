using Api.Dtos.Info;

namespace Api.Dtos.Customers;

public record NewCustomerDto(
    Guid UserId,
    string FullName,
    string Cpf,
    string Email,
    DateTime Birthdate,
    string Gender,
    List<NewPhoneDto> Phones
);
