using Api.Dtos.Customers;
using Api.Repositories;

namespace Api.Endpoints.Customers;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/customers");

        group.MapGet("/", async (ICustomerRepository repository, CancellationToken ct) =>
        {
            var customers = await repository.GetAllAsync(ct);
            return Results.Ok(customers);
        });

        group.MapGet("/{id:guid}", async (Guid id, ICustomerRepository repository, CancellationToken ct) =>
        {
            var customer = await repository.GetByIdAsync(id, ct);
            return customer is null ? Results.NotFound() : Results.Ok(customer);
        });

        group.MapPost("/", async (NewCustomerDto dto, ICustomerRepository repository, CancellationToken ct) =>
        {
            var created = await repository.CreateAsync(dto, ct);
            return Results.Created($"/customers/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateCustomerDto dto, ICustomerRepository repository, CancellationToken ct) =>
        {
            var updated = await repository.UpdateAsync(id, dto, ct);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICustomerRepository repository, CancellationToken ct) =>
        {
            var deleted = await repository.DeleteAsync(id, ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
