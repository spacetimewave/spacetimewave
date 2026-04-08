using Application.Configuration;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using System.Collections.Concurrent;

namespace Application.Services;

public class PaymentsService : IPaymentsService
{
    private readonly StripeSettings _stripeSettings;
    private readonly ILogger<PaymentsService> _logger;
    private readonly ConcurrentDictionary<string, Subscription> _subscriptions = new();

    public PaymentsService(StripeSettings stripeSettings, ILogger<PaymentsService> logger)
    {
        _stripeSettings = stripeSettings;
        _logger = logger;
        StripeConfiguration.ApiKey = stripeSettings.SecretKey;
    }

    public async Task<Result<string>> CreateCheckoutSessionAsync(string userId, string userEmail, string successUrl, string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            Mode = "subscription",
            CustomerEmail = userEmail,
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = _stripeSettings.ProPriceId,
                    Quantity = 1,
                }
            ],
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            Metadata = new Dictionary<string, string> { ["userId"] = userId },
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        _logger.LogInformation("Created Stripe checkout session {SessionId} for user {UserId}", session.Id, userId);
        return new Result<string> { Success = true, Data = session.Url };
    }

    public Result<Subscription> GetSubscription(string userId)
    {
        var subscription = _subscriptions.GetValueOrDefault(userId)
            ?? new Subscription { UserId = userId, Plan = SubscriptionPlan.Free, Status = SubscriptionStatus.Inactive };

        return new Result<Subscription> { Success = true, Data = subscription };
    }

    public async Task<Result<bool>> HandleWebhookAsync(string payload, string stripeSignature)
    {
        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(payload, stripeSignature, _stripeSettings.WebhookSecret);
        }
        catch (StripeException ex)
        {
            _logger.LogWarning("Stripe webhook signature validation failed: {Message}", ex.Message);
            return new Result<bool> { Success = false, Error = new Domain.Entities.Error { Message = "Invalid webhook signature" } };
        }

        switch (stripeEvent.Type)
        {
            case EventTypes.CheckoutSessionCompleted:
                var session = stripeEvent.Data.Object as Session;
                if (session?.Metadata.TryGetValue("userId", out var userId) == true && userId is not null)
                    ActivateSubscription(userId, session.CustomerId, session.SubscriptionId);
                break;

            case EventTypes.CustomerSubscriptionDeleted:
            case EventTypes.CustomerSubscriptionUpdated:
                var sub = stripeEvent.Data.Object as Subscription;
                if (sub is not null)
                    UpdateSubscriptionByStripeId(sub.Id, sub.Status);
                break;
        }

        return new Result<bool> { Success = true, Data = true };
    }

    private void ActivateSubscription(string userId, string? stripeCustomerId, string? stripeSubscriptionId)
    {
        var subscription = new Subscription
        {
            UserId = userId,
            Plan = SubscriptionPlan.Pro,
            Status = SubscriptionStatus.Active,
            StripeCustomerId = stripeCustomerId,
            StripeSubscriptionId = stripeSubscriptionId,
            UpdatedAt = DateTime.UtcNow,
        };
        _subscriptions[userId] = subscription;
        _logger.LogInformation("Activated PRO subscription for user {UserId}", userId);
    }

    private void UpdateSubscriptionByStripeId(string stripeSubscriptionId, string stripeStatus)
    {
        var entry = _subscriptions.Values.FirstOrDefault(s => s.StripeSubscriptionId == stripeSubscriptionId);
        if (entry is null) return;

        entry.Status = stripeStatus == "active" ? SubscriptionStatus.Active : SubscriptionStatus.Canceled;
        entry.Plan = entry.Status == SubscriptionStatus.Active ? SubscriptionPlan.Pro : SubscriptionPlan.Free;
        entry.UpdatedAt = DateTime.UtcNow;
        _logger.LogInformation("Updated subscription {StripeId} → {Status}", stripeSubscriptionId, entry.Status);
    }
}
