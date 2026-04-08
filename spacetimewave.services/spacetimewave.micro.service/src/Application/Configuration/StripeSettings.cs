namespace Application.Configuration;

public class StripeSettings
{
    // For local development and testing use a Stripe Sandbox
    // Stripe Sandbox: Recurring pricing model > Flat rate > Pre-built checkout form 
    // Create recurring product > Name: Pro Subscription, Currency: EUR, Recurring: Monthly, Price: $9.99
    public string SecretKey { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;
    public string ProSubscriptionProductId { get; set; } = string.Empty;
    public string ProSubscriptionPriceId { get; set; } = string.Empty;
    // WebhookSecret → you get this from the Stripe CLI, not the dashboard:
    // stripe listen --forward-to https://localhost:8443/api/payments/webhook
    public string WebhookSecret { get; set; } = string.Empty;
}