using Api.Dtos.Subscriptions;
using Api.Repositories;

namespace Api.Endpoints.Subscriptions;

public static class SubscriptionEndpoints
{
    public static void MapSubscriptionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/subscriptions");

        group.MapGet("/", async (ISubscriptionRepository repository, CancellationToken ct) =>
        {
            var subscriptions = await repository.GetAllAsync(ct);
            return Results.Ok(subscriptions);
        });

        group.MapGet("/{id:guid}", async (Guid id, ISubscriptionRepository repository, CancellationToken ct) =>
        {
            var subscription = await repository.GetByIdAsync(id, ct);
            return subscription is null ? Results.NotFound() : Results.Ok(subscription);
        });

        group.MapGet("/driver/{driverId:guid}", async (Guid driverId, ISubscriptionRepository repository, CancellationToken ct) =>
        {
            var subscription = await repository.GetByDriverIdAsync(driverId, ct);
            return subscription is null ? Results.NotFound() : Results.Ok(subscription);
        });

        group.MapPost("/", async (NewSubscriptionDto dto, ISubscriptionRepository repository, CancellationToken ct) =>
        {
            try
            {
                var created = await repository.CreateAsync(dto, ct);
                return Results.Created($"/subscriptions/{created.Id}", created);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { ex.Message });
            }
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateSubscriptionDto dto, ISubscriptionRepository repository, CancellationToken ct) =>
        {
            var updated = await repository.UpdateAsync(id, dto, ct);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISubscriptionRepository repository, CancellationToken ct) =>
        {
            var deleted = await repository.DeleteAsync(id, ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
