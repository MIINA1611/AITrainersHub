using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
    public class PlatformCountry
    {
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; } = null!;
        public Guid CountryId { get; set; }
        public Country Country { get; set; } = null!;

        // متاح بالكامل / متاح جزئياً (مثلاً مهام أقل أو أجر أقل في الدولة دي)
        public bool IsFullySupported { get; set; } = true;
        public string? Notes { get; set; }


        public PlatformCountry()
        {
            
        }
        public PlatformCountry(Guid platformId, Guid countryId, bool isFullySupported, string? notes)
        {
            PlatformId = platformId;
            CountryId = countryId;
            IsFullySupported = isFullySupported;
            Notes = notes;
        }
        public void Update(bool isFullySupported, string? notes)
        {
            IsFullySupported = isFullySupported;
            Notes = notes ?? Notes;
        }
    }
}
