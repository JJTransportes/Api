using Api.Enums;

namespace Api.Models.Info;

public class PhoneNumber
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public UserType UserType { get; set; }
  public string Number { get; set; } = string.Empty;
  public string AreaCode { get; set; } = string.Empty;
  public string CountryCode { get; set; } = string.Empty;
  public string Type { get; set; } = string.Empty;
}
