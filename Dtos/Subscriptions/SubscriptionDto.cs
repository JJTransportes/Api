using Api.Enums;

namespace Api.Dtos.Subscriptions;

public record SubscriptionDto(
    Guid Id,
    Guid DriverId,
    TaxMode TaxMode,
    SignatureStatus Status,
    int SplitPercentage,
    int SubscriptionPrice
);
