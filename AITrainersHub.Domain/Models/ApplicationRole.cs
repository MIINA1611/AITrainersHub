using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AITrainersHub.Domain.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; } // شرح مختصر: الدور ده يقدر يعمل إيه بالظبط

    }
}
