using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
    public class PaymentMethod
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        public ICollection<PlatformPaymentMethod> PlatformPaymentMethods { get; set; } = new List<PlatformPaymentMethod>();


        public PaymentMethod()
        {
            
        }
        public PaymentMethod(string name)
        {
            Name = name;
        }
        public void Update(string? name)
        {
            Name = name ?? Name;
        }
    }
}
