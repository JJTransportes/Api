using Api.Dtos.Subscriptions;
using Api.Models.Subscriptions;

namespace Api.Extensions.Subscriptions;

public static class SubscriptionExtensions
{
    public static SubscriptionDto MapToDto(this Subscription subscription)
    {
        return new SubscriptionDto(
            subscription.Id,
            subscription.UserId,
            subscription.TaxMode,
            subscription.Status,
            subscription.SplitPercentage,
            subscription.SubscriptionPrice
        );
    }
}
