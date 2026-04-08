using Domain.Entities;

namespace Application.Services;

public interface IPaymentsService
{
    Task<Result<string>> CreateCheckoutSessionAsync(string userId, string userEmail, string successUrl, string cancelUrl);
    Result<Subscription> GetSubscription(string userId);
    Task<Result<bool>> HandleWebhookAsync(string payload, string stripeSignature);
}
