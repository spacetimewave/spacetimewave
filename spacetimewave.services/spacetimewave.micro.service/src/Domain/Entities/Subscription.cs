namespace Domain.Entities;

public enum SubscriptionPlan { Free, Pro }

public enum SubscriptionStatus { Active, Inactive, Canceled }

public class Subscription
{
    public string UserId { get; set; } = string.Empty;
    public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Free;
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Inactive;
    public string? StripeCustomerId { get; set; }
    public string? StripeSubscriptionId { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
