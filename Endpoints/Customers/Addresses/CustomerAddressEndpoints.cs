using Api.Dtos.Info;
using Api.Repositories;

namespace Api.Endpoints.Customers.Addresses;

public static class CustomerAddressEndpoints
{
    public static void MapCustomerAddressEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/customers/{customerId:guid}/address");

        group.MapGet("/", async (Guid customerId, IAddressRepository repository, CancellationToken ct) =>
        {
            var address = await repository.GetByUserAsync(customerId, Enums.UserType.Customer, ct);
            return address is null ? Results.NotFound() : Results.Ok(address);
        });

        group.MapPost("/", async (Guid customerId, NewAddressDto dto, IAddressRepository repository, CancellationToken ct) =>
        {
            try
            {
                var created = await repository.CreateAsync(customerId, Enums.UserType.Customer, dto, ct);
                return Results.Created($"/customers/{customerId}/address", created);
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        });

        group.MapPut("/", async (Guid customerId, UpdateAddressDto dto, IAddressRepository repository, CancellationToken ct) =>
        {
            try
            {
                var updated = await repository.UpdateAsync(customerId, Enums.UserType.Customer, dto, ct);
                return Results.Ok(updated);
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        });

        group.MapDelete("/", async (Guid customerId, IAddressRepository repository, CancellationToken ct) =>
        {
            var deleted = await repository.DeleteAsync(customerId, Enums.UserType.Customer, ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
