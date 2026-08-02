using Api.Dtos.Admins;
using Api.Dtos.Info;
using Api.Models.Admins;
using Api.Models.Info;

namespace Api.Extensions.Admins;

  public static class AdminExtensions
  {
      
    public static AdminDto MapToDto(this Admin admin, List<PhoneNumber> phones)
    {
        return new AdminDto(
            admin.Id,
            admin.FullName,
            admin.Cpf,
            admin.Email,
            admin.Birthdate,
            admin.CreatedAt,
            admin.Approved,
            phones.Select(p => new PhoneDto(
                p.Id,
                p.Number,
                p.AreaCode,
                p.CountryCode,
                p.Type
            )).ToList()
        );
    }
  }