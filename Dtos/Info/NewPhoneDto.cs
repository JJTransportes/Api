namespace Api.Dtos.Info;

public record NewPhoneDto(
  string Number,
  string AreaCode,
  string CountryCode,
  string Type
);