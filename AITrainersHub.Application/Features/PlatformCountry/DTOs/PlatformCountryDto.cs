namespace AITrainersHub.Application.Features.PlatformCountry.DTOs
{
    /// <summary>
    /// Represents a country linked to a platform.
    /// </summary>
    public class PlatformCountryDto
    {
        public Guid CountryId { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string IsoCode { get; set; } = string.Empty;
        public bool IsFullySupported { get; set; }
        public string? Notes { get; set; }
    }
}
