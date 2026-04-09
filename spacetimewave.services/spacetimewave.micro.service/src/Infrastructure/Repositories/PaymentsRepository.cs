using Application.Configuration;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using System.Collections.Concurrent;

namespace Infrastructure.Repositories;

public class PaymentsRepository : IPaymentsRepository
{
    private readonly StripeSettings _stripeSettings;
    private readonly ILogger<IPaymentsRepository> _logger;
    private readonly ConcurrentDictionary<string, Domain.Entities.Subscription> _subscriptions = new();

    public PaymentsRepository(StripeSettings stripeSettings, ILogger<IPaymentsRepository> logger)
    {
        _stripeSettings = stripeSettings;
        _logger = logger;
        StripeConfiguration.ApiKey = stripeSettings.SecretKey;
    }

    public async Task<string> CreateCheckoutSessionAsync(string userId, string userEmail, string successUrl, string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            Mode = "subscription",
            CustomerEmail = userEmail,
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = _stripeSettings.ProSubscriptionPriceId,
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
        return session.Url;
    }

    public Domain.Entities.Subscription GetSubscription(string userId)
    {
        var subscription = _subscriptions.GetValueOrDefault(userId)
            ?? new Domain.Entities.Subscription { UserId = userId, Plan = SubscriptionPlan.Free, Status = SubscriptionStatus.Inactive };

        return subscription;
    }

    public async Task<bool> HandleWebhookAsync(string payload, string stripeSignature)
    {
        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(payload, stripeSignature, _stripeSettings.WebhookSecret);
        }
        catch (StripeException ex)
        {
            _logger.LogWarning("Stripe webhook signature validation failed: {Message}", ex.Message);
            return false;
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
                var sub = stripeEvent.Data.Object as Stripe.Subscription;
                if (sub is not null)
                    UpdateSubscriptionByStripeId(sub.Id, sub.Status);
                break;
        }

        return true;
    }

    private void ActivateSubscription(string userId, string? stripeCustomerId, string? stripeSubscriptionId)
    {
        var subscription = new Domain.Entities.Subscription
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
