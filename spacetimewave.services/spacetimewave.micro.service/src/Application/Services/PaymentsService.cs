using Application.Repositories;
using Domain.Entities;

namespace Application.Services;

public class PaymentsService : IPaymentsService
{

    private readonly IPaymentsRepository _paymentsRepository;
    public PaymentsService(IPaymentsRepository paymentsRepository)
    {
        _paymentsRepository = paymentsRepository;
    }

    public async Task<Result<string>> CreateCheckoutSessionAsync(string userId, string userEmail, string successUrl, string cancelUrl)
    {
        try
        {
            var sessionUrl = await _paymentsRepository.CreateCheckoutSessionAsync(userId, userEmail, successUrl, cancelUrl);
            return new Result<string> { Success = true, Data = sessionUrl };
        }
        catch (Exception ex)
        {
            Error error = new Error { Code= ErrorCode.InternalError, Message = ex.Message };
            return new Result<string> { Success = false, Error = error };
        }
    }

    public Result<Subscription> GetSubscription(string userId)
    {
        try
        {
            var subscription = _paymentsRepository.GetSubscription(userId);
            return new Result<Subscription> { Success = true, Data = subscription };
        }
        catch (Exception ex)
        {
            Error error = new Error { Code= ErrorCode.InternalError, Message = ex.Message };
            return new Result<Subscription> { Success = false, Error = error };
        }
    }

    public async Task<Result<bool>> HandleWebhookAsync(string payload, string stripeSignature)
    {
        try
        {
            bool success = await _paymentsRepository.HandleWebhookAsync(payload, stripeSignature);
            return new Result<bool> { Success = success, Data = success };
        }
        catch (Exception ex)
        {
            Error error = new Error { Code= ErrorCode.InternalError, Message = ex.Message };
            return new Result<bool> { Success = false, Error = error };
        }
    }
}
