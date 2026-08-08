using Api.Enums;

namespace Api.Dtos.Subscriptions;

public record UpdateSubscriptionDto(
    TaxMode? TaxMode,
    SignatureStatus? Status,
    int? SplitPercentage,
    int? SubscriptionPrice
);
