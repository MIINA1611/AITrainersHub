using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
    public class Country
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string IsoCode { get; set; } = string.Empty; // "EG", "SA", "AE" ...

        public ICollection<PlatformCountry> PlatformCountries { get; set; } = new List<PlatformCountry>();


        public Country()
        {
            
        }
        public Country(string nameAr, string nameEn, string isoCode)
        {
            NameAr = nameAr;
            NameEn = nameEn;
            IsoCode = isoCode;
        }
        public void Update(string? nameAr, string? nameEn, string? isoCode)
        {
            NameAr = nameAr ?? NameAr;
            NameEn = nameEn ?? NameEn;
            IsoCode = isoCode ?? IsoCode;
        }
    }
}
