using Api.Dtos.Info;
using Api.Repositories;

namespace Api.Endpoints.Drivers.Addresses;

public static class DriverAddressEndpoints
{
    public static void MapDriverAddressEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/drivers/{driverId:guid}/address");

        group.MapGet("/", async (Guid driverId, IAddressRepository repository, CancellationToken ct) =>
        {
            var address = await repository.GetByUserAsync(driverId, Enums.UserType.Driver, ct);
            return address is null ? Results.NotFound() : Results.Ok(address);
        });

        group.MapPost("/", async (Guid driverId, NewAddressDto dto, IAddressRepository repository, CancellationToken ct) =>
        {
            try
            {
                var created = await repository.CreateAsync(driverId, Enums.UserType.Driver, dto, ct);
                return Results.Created($"/drivers/{driverId}/address", created);
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        });

        group.MapPut("/", async (Guid driverId, UpdateAddressDto dto, IAddressRepository repository, CancellationToken ct) =>
        {
            try
            {
                var updated = await repository.UpdateAsync(driverId, Enums.UserType.Driver, dto, ct);
                return Results.Ok(updated);
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        });

        group.MapDelete("/", async (Guid driverId, IAddressRepository repository, CancellationToken ct) =>
        {
            var deleted = await repository.DeleteAsync(driverId, Enums.UserType.Driver, ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
