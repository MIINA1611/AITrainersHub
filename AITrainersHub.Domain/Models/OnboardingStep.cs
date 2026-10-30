using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Domain.Models
{
    public class OnboardingStep
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; } = null!;

        public int StepNumber { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string? Tip { get; set; }            // ملحوظة سريعة اختيارية تحت الخطوة
        public string? ScreenshotUrl { get; set; }  // اختياري لبعدين


        public OnboardingStep()
        {
            
        }
        public OnboardingStep(
            Guid platformId,
            int stepNumber,
            string titleAr,
            string titleEn,
            string descriptionAr,
            string descriptionEn,
            string? tip,
            string? screenshotUrl)
        {
            PlatformId = platformId;
            StepNumber = stepNumber;
            TitleAr = titleAr;
            TitleEn = titleEn;
            DescriptionAr = descriptionAr;
            DescriptionEn = descriptionEn;
            Tip = tip;
            ScreenshotUrl = screenshotUrl;
        }
        public void Update(
            int? stepNumber,
            string? titleAr,
            string? titleEn,
            string? descriptionAr,
            string? descriptionEn,
            string? tip,
            string? screenshotUrl)
        {
            StepNumber = stepNumber ?? StepNumber;
            TitleAr = titleAr ?? TitleAr;
            TitleEn = titleEn ?? TitleEn;
            DescriptionAr = descriptionAr ?? DescriptionAr;
            DescriptionEn = descriptionEn ?? DescriptionEn;
            Tip = tip ?? Tip;
            ScreenshotUrl = screenshotUrl ?? ScreenshotUrl;
        }
    }
}
