using Api.Enums;

namespace Api.Models.Subscriptions;

public class Subscription
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public TaxMode TaxMode { get; set; }
    public SignatureStatus Status { get; set; }
    public int SplitPercentage { get; set; }
    public int SubscriptionPrice { get; set; }
}
