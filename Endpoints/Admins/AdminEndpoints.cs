using Api.Dtos.Admins;
using Api.Repositories;

namespace Api.Endpoints.Admins;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/admins");

        group.MapGet("/", async (IAdminRepository repository, CancellationToken ct) =>
        {
            var admins = await repository.GetAllAsync(ct);
            return Results.Ok(admins);
        });

        group.MapGet("/{id:guid}", async (Guid id, IAdminRepository repository, CancellationToken ct) =>
        {
            var admin = await repository.GetByIdAsync(id, ct);
            return admin is null ? Results.NotFound() : Results.Ok(admin);
        });

        group.MapPost("/", async (NewAdminDto dto, IAdminRepository repository, CancellationToken ct) =>
        {
            var created = await repository.CreateAsync(dto, ct);
            return Results.Created($"/admins/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateAdminDto dto, IAdminRepository repository, CancellationToken ct) =>
        {
            var updated = await repository.UpdateAsync(id, dto, ct);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        group.MapDelete("/{id:guid}", async (Guid id, IAdminRepository repository, CancellationToken ct) =>
        {
            var deleted = await repository.DeleteAsync(id, ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
