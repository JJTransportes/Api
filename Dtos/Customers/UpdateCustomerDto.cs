using Api.Dtos.Info;

namespace Api.Dtos.Customers;

public record UpdateCustomerDto(
    string? FullName,
    string? Email,
    DateTime? Birthdate,
    string? Gender,
    bool? Approved,
    List<NewPhoneDto>? Phones
);
