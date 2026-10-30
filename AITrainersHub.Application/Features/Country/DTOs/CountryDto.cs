namespace AITrainersHub.Application.Features.Country.DTOs
{
    /// <summary>
    /// Represents a country returned by the country queries and commands.
    /// </summary>
    public class CountryDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string IsoCode { get; set; } = string.Empty;
    }
}
