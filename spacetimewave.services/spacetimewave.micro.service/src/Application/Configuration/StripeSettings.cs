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
    
    // Descarga el archivo ZIP windows más reciente desde GitHub.
    // Descomprime el archivo stripe_X.X.X_windows_x86_64.zip.
    // Añade la ruta al archivo descomprimido stripe.exe a la variable de entorno Path (C:\Program Files\Stripe)
    
    // stripe listen --forward-to https://localhost:9080/api/payments/webhook
    // You have not configured API keys yet. Running `stripe login`...
    // Your pairing code is: trump-zenith-openly-yay
    // This pairing code verifies your authentication with Stripe.
    // Press Enter to open the browser or visit https://dashboard.stripe.com/stripecli/confirm_auth?t=...
    public string WebhookSecret { get; set; } = string.Empty;
}