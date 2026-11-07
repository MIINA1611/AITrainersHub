using AITrainerHub.ViewModels;
using AITrainersHub.Application.Features.Categories.DTOs;
using AITrainersHub.Application.Features.Country.DTOs;
using AITrainersHub.Application.Features.PaymentMethod.DTOs;
using AITrainersHub.Application.Features.Platform.DTOs;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainerHub.Presentation
{
    /// <summary>
    /// Applies the public directory filters in memory so the home page and the
    /// platforms page always show identical results for the same query string.
    /// </summary>
    public static class PlatformDirectoryBuilder
    {
        public const int PageSize = 9;

        public static PlatformDirectoryViewModel Build(
            IEnumerable<PlatformListResponse> platforms,
            List<CategoryListResponse> categories,
            List<CountryDto> countries,
            List<PaymentMethodDto> paymentMethods,
            string? search,
            string? status,
            Guid? category,
            Guid? country,
            Guid? payment,
            int page)
        {
            PlatformStatus? statusFilter = null;
            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<PlatformStatus>(status, true, out var parsedStatus))
            {
                statusFilter = parsedStatus;
            }

            IEnumerable<PlatformListResponse> filtered = platforms;

            if (statusFilter.HasValue)
            {
                filtered = filtered.Where(platform => platform.Status == statusFilter.Value);
            }

            if (category.HasValue)
            {
                filtered = filtered.Where(platform =>
                    platform.Categories.Any(item => item.CategoryId == category.Value));
            }

            if (country.HasValue)
            {
                filtered = filtered.Where(platform =>
                    platform.Countries.Any(item => item.CountryId == country.Value));
            }

            if (payment.HasValue)
            {
                filtered = filtered.Where(platform =>
                    platform.PaymentMethods.Any(item => item.PaymentMethodId == payment.Value));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                filtered = filtered.Where(platform =>
                    platform.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    platform.Slug.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            var filteredList = filtered
                .OrderBy(platform => platform.Name)
                .ToList();

            if (page < 1)
            {
                page = 1;
            }

            var totalPages = filteredList.Count == 0
                ? 1
                : (int)Math.Ceiling(filteredList.Count / (double)PageSize);

            if (page > totalPages)
            {
                page = totalPages;
            }

            return new PlatformDirectoryViewModel
            {
                Platforms = filteredList
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                Categories = categories,
                Countries = countries,
                PaymentMethods = paymentMethods,
                Search = search,
                Status = statusFilter,
                CategoryId = category,
                CountryId = country,
                PaymentMethodId = payment,
                Page = page,
                PageSize = PageSize,
                TotalCount = filteredList.Count,
                TotalPages = totalPages
            };
        }
    }
}
