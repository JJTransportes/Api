using Api.Enums;

namespace Api.Dtos.Subscriptions;

public record NewSubscriptionDto(
    Guid UserId,
    TaxMode TaxMode
);
