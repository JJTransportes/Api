using Api.Enums;
using Api.Models.Info;

namespace Api.Models.Drivers;

public class Driver
{
  public Guid Id { get; set; }
  public string FullName { get; set; } = string.Empty;
  public string Cpf { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public UserType UserType { get; set; } = UserType.Driver;
  public DateTime Birthdate { get; set; }
  public DateTime CreatedAt { get; set; }
  public string Gender { get; set; } = string.Empty;
  public bool Approved { get; set; } = false;
  public ICollection<PhoneNumber> Phones { get; set; } = new List<PhoneNumber>();
  public Address? Address { get; set; }
}
