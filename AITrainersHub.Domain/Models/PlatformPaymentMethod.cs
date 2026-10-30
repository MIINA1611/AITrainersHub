using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
    public class PlatformPaymentMethod
    {
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; } = null!;
        public Guid PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = null!;


        public PlatformPaymentMethod()
        {
            
        }
        public PlatformPaymentMethod(Guid platformId, Guid paymentMethodId)
        {
            PlatformId = platformId;
            PaymentMethodId = paymentMethodId;
        }
    }
}
