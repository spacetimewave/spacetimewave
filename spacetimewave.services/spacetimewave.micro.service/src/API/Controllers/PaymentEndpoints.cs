using Application.Services;
using Domain.Entities;
using System.Security.Claims;

namespace API.Controllers;

public static class PaymentEndpoints
{
    public sealed class PaymentsLogger;

    public static void MapPaymentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/payments");
        group.MapPost("checkout", CreateCheckout).WithName("CreateCheckout").RequireAuthorization();
        group.MapGet("subscription", GetSubscription).WithName("GetSubscription").RequireAuthorization();
        group.MapPost("webhook", HandleWebhook).WithName("HandleWebhook");
    }

    public static async Task<IResult> CreateCheckout(
        CheckoutRequest request,
        ClaimsPrincipal user,
        IPaymentsService paymentsService,
        ILogger<PaymentsLogger> logger)
    {
        string? userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string? userEmail = user.FindFirst(ClaimTypes.Email)?.Value
            ?? user.FindFirst("email")?.Value
            ?? user.FindFirst("preferred_username")?.Value;

        if (userId is null) return Results.Unauthorized();

        var result = await paymentsService.CreateCheckoutSessionAsync(userId, userEmail ?? string.Empty, request.SuccessUrl, request.CancelUrl);

        if (!result.Success)
        {
            logger.LogError("Failed to create checkout session for user {UserId}", userId);
            return Results.Problem("Failed to create checkout session.");
        }

        return Results.Ok(new { url = result.Data });
    }

    public static IResult GetSubscription(
        ClaimsPrincipal user,
        IPaymentsService paymentsService)
    {
        string? userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Results.Unauthorized();

        var result = paymentsService.GetSubscription(userId);
        return Results.Ok(result.Data);
    }

    public static async Task<IResult> HandleWebhook(
        HttpRequest httpRequest,
        IPaymentsService paymentsService,
        ILogger<PaymentsLogger> logger)
    {
        string payload;
        using (var reader = new StreamReader(httpRequest.Body))
            payload = await reader.ReadToEndAsync();

        string? stripeSignature = httpRequest.Headers["Stripe-Signature"];
        if (stripeSignature is null)
            return Results.BadRequest("Missing Stripe-Signature header.");

        var result = await paymentsService.HandleWebhookAsync(payload, stripeSignature);

        if (!result.Success)
        {
            logger.LogWarning("Webhook processing failed");
            return Results.BadRequest(result.Error?.Message);
        }

        return Results.Ok();
    }

    public record CheckoutRequest(string SuccessUrl, string CancelUrl);
}
