using AITrainerHub.Request_Models;
using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.DTOs;

namespace AITrainerHub.ViewModels
{
    /// <summary>
    /// Data required by the Payment Methods reference data tab.
    /// </summary>
    public class PaymentMethodsViewModel
    {
        public List<PaymentMethodDto> Items { get; set; } = new();
        public PaymentMethodFormRequest NewItem { get; set; } = new();
    }

    /// <summary>
    /// Data required by the Countries reference data tab.
    /// </summary>
    public class CountriesViewModel
    {
        public List<CountryDto> Items { get; set; } = new();
        public CountryFormRequest NewItem { get; set; } = new();
    }
}
