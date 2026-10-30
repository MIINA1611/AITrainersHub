using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PaymentMethod.Queries
{
    /// <summary>
    /// Represents a query to read all payment methods.
    /// </summary>
    public class ReadPaymentMethodsQuery : IRequest<List<PaymentMethodDto>>, ICacheableQuery
    {
        public string CacheKey => nameof(ReadPaymentMethodsQuery);
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
