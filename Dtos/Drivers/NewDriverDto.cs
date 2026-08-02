using Api.Dtos.Info;

namespace Api.Dtos.Drivers;

public record NewDriverDto(
    string FullName,
    string Cpf,
    string Email,
    DateTime Birthdate,
    string Gender,
    List<NewPhoneDto> Phones
);
