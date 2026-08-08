using Api.Dtos.Info;

namespace Api.Dtos.Admins;

public record NewAdminDto(
  Guid UserId,
  string FullName,
  string Cpf,
  string Email,
  DateTime Birthdate,
  List<NewPhoneDto> Phones
);