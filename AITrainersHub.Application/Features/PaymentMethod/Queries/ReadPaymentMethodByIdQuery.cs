using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.Features.PaymentMethod.Queries
{
    /// <summary>
    /// Represents a query to read a payment method by its unique identifier.
    /// </summary>
    public class ReadPaymentMethodByIdQuery : IRequest<PaymentMethodDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"{nameof(ReadPaymentMethodByIdQuery)}_{Id}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public ReadPaymentMethodByIdQuery()
        {
        }

        public ReadPaymentMethodByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
