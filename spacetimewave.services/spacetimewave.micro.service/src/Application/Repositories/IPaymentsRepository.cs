using Domain.Entities;

namespace Application.Repositories;

public interface IPaymentsRepository
{
    Task<string> CreateCheckoutSessionAsync(string userId, string userEmail, string successUrl, string cancelUrl);
    Subscription GetSubscription(string userId);
    Task<bool> HandleWebhookAsync(string payload, string stripeSignature);
}
