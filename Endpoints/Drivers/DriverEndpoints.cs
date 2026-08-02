using Api.Dtos.Drivers;
using Api.Repositories;

namespace Api.Endpoints.Drivers;

public static class DriverEndpoints
{
    public static void MapDriverEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/drivers");

        group.MapGet("/", async (IDriverRepository repository, CancellationToken ct) =>
        {
            var drivers = await repository.GetAllAsync(ct);
            return Results.Ok(drivers);
        });

        group.MapGet("/{id:guid}", async (Guid id, IDriverRepository repository, CancellationToken ct) =>
        {
            var driver = await repository.GetByIdAsync(id, ct);
            return driver is null ? Results.NotFound() : Results.Ok(driver);
        });

        group.MapPost("/", async (NewDriverDto dto, IDriverRepository repository, CancellationToken ct) =>
        {
            var created = await repository.CreateAsync(dto, ct);
            return Results.Created($"/drivers/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateDriverDto dto, IDriverRepository repository, CancellationToken ct) =>
        {
            var updated = await repository.UpdateAsync(id, dto, ct);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        group.MapDelete("/{id:guid}", async (Guid id, IDriverRepository repository, CancellationToken ct) =>
        {
            var deleted = await repository.DeleteAsync(id, ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
