using AITrainersHub.Domain.Enums;
using AITrainersHub.Domain.Models;
using AITrainersHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlatformStatus = AITrainersHub.Domain.Enums.PlatfromStatus.PlatformStatus;

namespace AITrainersHub.Infrastructure.DbInitializer
{
    /// <summary>
    /// Seeds a realistic, idempotent demo catalogue for local inspection. All
    /// rows are matched on stable natural keys before they are inserted, so
    /// restarting the application does not create duplicates.
    /// </summary>
    public static class DemoDataSeeder
    {
        private const string DefaultAdminEmail = "twins.jack26@gmail.com";
        private const string DefaultAdminPassword = "Admin@123";
        private const string LeaderPassword = "Leader@123";

        private sealed record CategorySeed(string Name, string Description, string[] LegacyNames);

        private sealed record CountrySeed(string IsoCode, string NameEn, string NameAr);

        private sealed record PaymentMethodSeed(string Name, string[] LegacyNames);

        private sealed record CountryLinkSeed(string IsoCode, bool IsFullySupported, string? Notes);

        private sealed record PlatformSeed(
            string Name,
            string Slug,
            string DescriptionEn,
            string DescriptionAr,
            string WebsiteUrl,
            PlatformStatus Status,
            int CreatedDaysAgo,
            string[] Categories,
            CountryLinkSeed[] Countries,
            string[] PaymentMethods);

        private sealed record OnboardingTemplate(
            int StepNumber,
            string TitleEn,
            string TitleAr,
            string DescriptionEn,
            string DescriptionAr,
            string Tip);

        private sealed record VideoSeed(
            string Title,
            GuideLanguage Language,
            string Url,
            string Author,
            int PublishedDaysAgo);

        private sealed record OpportunitySeed(
            string Title,
            string LeaderEmail,
            string PlatformSlug,
            string DescriptionEn,
            string DescriptionAr,
            string WhatsAppLink,
            bool IsApproved,
            int CreatedDaysAgo);

        private sealed record ReviewSeed(
            string PlatformSlug,
            string ReviewerName,
            int Rating,
            string Comment,
            bool IsApproved,
            int CreatedDaysAgo);

        private sealed record SuggestionSeed(
            string PlatformName,
            string Url,
            string Description,
            string SubmitterName,
            string CategoryName,
            string? Email,
            int CreatedDaysAgo);

        private sealed record AdvertisementSeed(
            string Title,
            string Subtitle,
            string CtaText,
            string CtaUrl,
            string Position);

        private static readonly CategorySeed[] CategorySeeds =
        {
            new(
                "Coding & Development",
                "Python, JavaScript, data structures, and software engineering tasks.",
                new[] { "Coding" }),
            new(
                "Arabic Writing & Localization",
                "Arabic copywriting, translation quality, and cultural nuance.",
                new[] { "Content Writing" }),
            new(
                "Math & Logic Reasoning",
                "STEM problems, mathematical proofs, and step-by-step reasoning.",
                Array.Empty<string>()),
            new(
                "Audio & Speech Evaluation",
                "Transcription, pronunciation, and speech quality review.",
                new[] { "Audion" }),
            new(
                "RLHF & Model Alignment",
                "Preference ranking, red-teaming, and safer model behaviour.",
                Array.Empty<string>()),
            new(
                "General Insights",
                "Surveys, data labeling, and general human feedback tasks.",
                Array.Empty<string>())
        };

        private static readonly CountrySeed[] CountrySeeds =
        {
            new("EG", "Egypt", "مصر"),
            new("SA", "Saudi Arabia", "المملكة العربية السعودية"),
            new("AE", "United Arab Emirates", "الإمارات العربية المتحدة"),
            new("JO", "Jordan", "الأردن"),
            new("MA", "Morocco", "المغرب"),
            new("US", "United States", "الولايات المتحدة"),
            new("GB", "United Kingdom", "المملكة المتحدة"),
            new("DE", "Germany", "ألمانيا")
        };

        // The payment-method entity currently has a single label column, so the
        // seeded labels intentionally carry both the English and Arabic names.
        private static readonly PaymentMethodSeed[] PaymentMethodSeeds =
        {
            new("PayPal — باي بال", new[] { "PayPal Transfer", "PayPal" }),
            new("Payoneer — بايونير", new[] { "Payoneer" }),
            new("Bank Transfer — تحويل بنكي", new[] { "Bank Transfer" }),
            new("AirTM — إير تي إم", new[] { "AirTM", "Airtm" }),
            new("USDT (Crypto) — عملة USDT", new[] { "USDT (Crypto)", "USDT" })
        };

        private static readonly PlatformSeed[] PlatformSeeds =
        {
            new(
                "Outlier",
                "outlier",
                "Outlier connects subject-matter experts with flexible remote projects that improve leading generative AI models. Contributors work on coding, reasoning, multilingual writing, and evaluation tasks with weekly pay and a fully remote workflow.",
                "تربط Outlier الخبراء في مختلف المجالات بمشاريع عن بُعد مرنة لتحسين نماذج الذكاء الاصطناعي التوليدي الرائدة. يعمل المساهمون على مهام البرمجة والاستدلال والكتابة متعددة اللغات والتقييم، مع دفع أسبوعي وإمكانية العمل من أي مكان.",
                "https://outlier.ai/",
                PlatformStatus.Open,
                150,
                new[]
                {
                    "Coding & Development",
                    "Math & Logic Reasoning",
                    "RLHF & Model Alignment",
                    "General Insights"
                },
                new[]
                {
                    new CountryLinkSeed("US", true, "W-9 required; USD payouts via PayPal or AirTM."),
                    new CountryLinkSeed("GB", true, "Right-to-work verification may be requested."),
                    new CountryLinkSeed("DE", true, "EU tax ID required for invoices."),
                    new CountryLinkSeed("AE", true, "Supported; local bank transfer available."),
                    new CountryLinkSeed("SA", true, "English proficiency required; project availability varies."),
                    new CountryLinkSeed("EG", false, "Limited project availability; PayPal payout supported.")
                },
                new[]
                {
                    "PayPal — باي بال",
                    "AirTM — إير تي إم",
                    "Bank Transfer — تحويل بنكي",
                    "USDT (Crypto) — عملة USDT"
                }),
            new(
                "DataAnnotation",
                "dataannotation",
                "DataAnnotation is a remote AI training platform where detail-oriented freelancers rate model responses, write high-quality prompts, and solve coding or writing tasks. Work is self-paced and paid per completed project.",
                "DataAnnotation منصة تدريب ذكاء اصطناعي عن بُعد، يقيّم فيها المستقلون المهتمون بالتفاصيل استجابات النماذج ويكتبون أوامر عالية الجودة ويحلون مهام البرمجة أو الكتابة. العمل ذاتي الوتيرة ويُدفع مقابل كل مشروع مكتمل.",
                "https://www.dataannotation.tech/",
                PlatformStatus.Open,
                140,
                new[]
                {
                    "Coding & Development",
                    "Arabic Writing & Localization",
                    "General Insights"
                },
                new[]
                {
                    new CountryLinkSeed("US", true, "Primary hiring region with the fastest project allocation."),
                    new CountryLinkSeed("GB", true, "Supported; GBP payouts available."),
                    new CountryLinkSeed("DE", true, "Supported for EU-based contributors.")
                },
                new[]
                {
                    "PayPal — باي بال"
                }),
            new(
                "Mercor",
                "mercor",
                "Mercor matches vetted AI trainers with fast-growing AI labs. Experts contribute to coding benchmarks, STEM reasoning datasets, and domain-specific evaluations through structured, paid engagements.",
                "تصل Mercor بين مدربي الذكاء الاصطناعي المعتمدين ومختبرات الذكاء الاصطناعي سريعة النمو. يساهم الخبراء في معايير البرمجة ومجموعات بيانات الاستدلال في مجالات العلوم والتقنية والتقييمات المتخصصة من خلال مشاريع مدفوعة ومنظمة.",
                "https://www.mercor.com/",
                PlatformStatus.Open,
                120,
                new[]
                {
                    "Coding & Development",
                    "Math & Logic Reasoning",
                    "Audio & Speech Evaluation"
                },
                new[]
                {
                    new CountryLinkSeed("US", true, "Core market; most STEM projects are allocated here."),
                    new CountryLinkSeed("GB", true, "Supported; remote interviews required."),
                    new CountryLinkSeed("AE", true, "Supported for Gulf-based experts."),
                    new CountryLinkSeed("SA", true, "Supported; English and Arabic projects."),
                    new CountryLinkSeed("EG", true, "Supported for selected technical roles."),
                    new CountryLinkSeed("JO", true, "Supported for selected technical roles.")
                },
                new[]
                {
                    "Bank Transfer — تحويل بنكي",
                    "USDT (Crypto) — عملة USDT",
                    "PayPal — باي بال"
                }),
            new(
                "Alignerr",
                "alignerr",
                "Alignerr is a community of AI trainers, linguists, and domain experts who create high-quality training data for frontier models. Projects span Arabic localization, RLHF, red-teaming, and specialist evaluation.",
                "Alignerr مجتمع من مدربي الذكاء الاصطناعي واللغويين والخبراء المتخصصين الذين ينشئون بيانات تدريب عالية الجودة للنماذج المتقدمة. تشمل المشاريع التعريب العربي وRLHF والاختبار الأحمر والتقييم المتخصص.",
                "https://www.alignerr.com/",
                PlatformStatus.Soon,
                90,
                new[]
                {
                    "Arabic Writing & Localization",
                    "RLHF & Model Alignment",
                    "General Insights"
                },
                new[]
                {
                    new CountryLinkSeed("US", true, "Core market for English-language evaluation projects."),
                    new CountryLinkSeed("GB", true, "Supported; right-to-work verification required."),
                    new CountryLinkSeed("AE", true, "Arabic localization projects available."),
                    new CountryLinkSeed("SA", true, "Arabic localization projects available.")
                },
                new[]
                {
                    "PayPal — باي بال",
                    "Payoneer — بايونير",
                    "AirTM — إير تي إم"
                }),
            new(
                "Mindrift",
                "mindrift",
                "Mindrift invites freelancers to shape the next generation of AI through short, flexible missions. Contributors write prompts, review multilingual content, and provide expert feedback from anywhere.",
                "تدعو Mindrift المستقلين للمساهمة في الجيل القادم من الذكاء الاصطناعي عبر مهام قصيرة ومرنة. يكتب المساهمون الأوامر ويراجعون المحتوى متعدد اللغات ويقدمون ملاحظات متخصصة من أي مكان.",
                "https://mindrift.ai/",
                PlatformStatus.Open,
                80,
                new[]
                {
                    "Arabic Writing & Localization",
                    "Math & Logic Reasoning",
                    "General Insights"
                },
                new[]
                {
                    new CountryLinkSeed("EG", true, "Arabic and English missions available."),
                    new CountryLinkSeed("SA", true, "Arabic writing and review missions."),
                    new CountryLinkSeed("AE", true, "Arabic and English missions available."),
                    new CountryLinkSeed("JO", true, "Arabic writing and review missions."),
                    new CountryLinkSeed("MA", false, "French and Arabic missions; limited availability."),
                    new CountryLinkSeed("US", true, "English-language missions available."),
                    new CountryLinkSeed("GB", true, "English-language missions available.")
                },
                new[]
                {
                    "PayPal — باي بال",
                    "Payoneer — بايونير",
                    "Bank Transfer — تحويل بنكي"
                }),
            new(
                "Telus Digital",
                "telus-digital",
                "Telus Digital's AI Community offers remote data annotation, transcription, and evaluation projects for global technology partners. Contributors receive structured guidelines, quality support, and flexible schedules.",
                "يوفر مجتمع الذكاء الاصطناعي في Telus Digital مشاريع عن بُعد لتعليقات البيانات والنسخ والتقييم لشركاء تقنيين حول العالم. يحصل المساهمون على إرشادات منظمة ودعم للجودة وجداول مرنة.",
                "https://www.telusdigital.com/",
                PlatformStatus.Closed,
                60,
                new[]
                {
                    "Audio & Speech Evaluation",
                    "General Insights",
                    "RLHF & Model Alignment"
                },
                new[]
                {
                    new CountryLinkSeed("EG", true, "Arabic transcription and annotation projects."),
                    new CountryLinkSeed("SA", true, "Arabic projects; local bank transfer supported."),
                    new CountryLinkSeed("AE", true, "Arabic and English projects."),
                    new CountryLinkSeed("JO", true, "Arabic projects; remote onboarding."),
                    new CountryLinkSeed("MA", false, "French and Arabic projects; limited capacity."),
                    new CountryLinkSeed("US", true, "English transcription and evaluation projects."),
                    new CountryLinkSeed("GB", true, "English projects; right-to-work checks."),
                    new CountryLinkSeed("DE", false, "German-language projects only.")
                },
                new[]
                {
                    "Bank Transfer — تحويل بنكي",
                    "PayPal — باي بال"
                })
        };

        private static readonly OnboardingTemplate[] OnboardingTemplates =
        {
            new(
                1,
                "Create your {platform} account",
                "أنشئ حسابك على {platform}",
                "Register with a professional email address and choose the country where you are legally eligible to work. Use your real name so it matches your identity documents.",
                "سجّل باستخدام بريد إلكتروني احترافي واختر الدولة التي يحق لك العمل منها قانونيًا. استخدم اسمك الحقيقي ليتطابق مع مستندات الهوية.",
                "Use a professional email and the name shown on your ID. / استخدم بريدًا احترافيًا والاسم الظاهر في هويتك."),
            new(
                2,
                "Complete your expert profile",
                "أكمل ملف الخبير",
                "Add your areas of expertise, languages, education, and weekly availability. A complete profile helps the platform match you with the most relevant projects.",
                "أضف مجالات خبرتك واللغات ومؤهلاتك وعدد الساعات الأسبوعية المتاحة. الملف المكتمل يساعد المنصة على مطابقتك مع المشاريع الأنسب.",
                "Be specific about your skills and available hours. / كن محددًا في مهاراتك وساعاتك المتاحة."),
            new(
                3,
                "Verify your identity and payout details",
                "وثّق هويتك وبيانات الدفع",
                "Complete the secure identity and payment verification flow. Keep your legal name, address, and tax information consistent across all documents.",
                "أكمل مسار التحقق الآمن من الهوية والدفع. حافظ على تطابق اسمك القانوني وعنوانك وبياناتك الضريبية في جميع المستندات.",
                "Prepare a clear ID photo and a valid payment account. / جهّز صورة هوية واضحة وحساب دفع صالحًا."),
            new(
                4,
                "Pass the skills assessment",
                "اجتز اختبار المهارات",
                "Read every instruction before answering. Show your reasoning step by step, follow the style guide, and never include personal or copyrighted information.",
                "اقرأ كل التعليمات قبل الإجابة. أظهر خطوات تفكيرك بوضوح واتبع دليل الأسلوب ولا تضمّن معلومات شخصية أو محمية بحقوق النشر.",
                "Quality beats speed in the assessment. / الجودة أهم من السرعة في الاختبار."),
            new(
                5,
                "Start your first project",
                "ابدأ مشروعك الأول",
                "Accept a small project, review the task guidelines, and submit a careful sample. Track reviewer feedback and keep response times within the platform's expected window.",
                "اقبل مشروعًا صغيرًا وراجع إرشادات المهمة وقدّم نموذجًا دقيقًا. تابع ملاحظات المراجعين وحافظ على أوقات استجابة ضمن المدة المتوقعة في المنصة.",
                "Ask for clarification early instead of guessing. / اطلب التوضيح مبكرًا بدلًا من التخمين.")
        };

        private static readonly Dictionary<string, VideoSeed[]> VideoSeeds = new(StringComparer.OrdinalIgnoreCase)
        {
            ["outlier"] = new[]
            {
                new VideoSeed(
                    "Outlier AI onboarding: application, assessment, and first project",
                    GuideLanguage.English,
                    "https://www.youtube.com/results?search_query=outlier+ai+onboarding+guide",
                    "AI Trainers Hub",
                    40),
                new VideoSeed(
                    "دليل Outlier للمدربين العرب: من التسجيل إلى أول مهمة",
                    GuideLanguage.Arabic,
                    "https://www.youtube.com/results?search_query=outlier+ai+arabic+tutorial",
                    "AI Trainers Hub Arabic",
                    32)
            },
            ["dataannotation"] = new[]
            {
                new VideoSeed(
                    "How to pass the DataAnnotation starter assessment",
                    GuideLanguage.English,
                    "https://www.youtube.com/results?search_query=dataannotation+starter+assessment",
                    "AI Trainers Hub",
                    36),
                new VideoSeed(
                    "شرح DataAnnotation بالعربية: التسجيل والتقييم",
                    GuideLanguage.Arabic,
                    "https://www.youtube.com/results?search_query=dataannotation+arabic+tutorial",
                    "AI Trainers Hub Arabic",
                    28)
            },
            ["mercor"] = new[]
            {
                new VideoSeed(
                    "Mercor profile, interview, and project matching walkthrough",
                    GuideLanguage.English,
                    "https://www.youtube.com/results?search_query=mercor+ai+trainer+interview",
                    "AI Trainers Hub",
                    34),
                new VideoSeed(
                    "كيف تجهز ملفك على Mercor وتجتاز المقابلة",
                    GuideLanguage.Arabic,
                    "https://www.youtube.com/results?search_query=mercor+arabic+guide",
                    "AI Trainers Hub Arabic",
                    24)
            },
            ["alignerr"] = new[]
            {
                new VideoSeed(
                    "Alignerr AI trainer application and onboarding guide",
                    GuideLanguage.English,
                    "https://www.youtube.com/results?search_query=alignerr+ai+trainer+guide",
                    "AI Trainers Hub",
                    30),
                new VideoSeed(
                    "دليل Alignerr بالعربية: التسجيل ومشاريع التعريب",
                    GuideLanguage.Arabic,
                    "https://www.youtube.com/results?search_query=alignerr+arabic+localization",
                    "AI Trainers Hub Arabic",
                    22)
            },
            ["mindrift"] = new[]
            {
                new VideoSeed(
                    "Mindrift onboarding: your first AI mission",
                    GuideLanguage.English,
                    "https://www.youtube.com/results?search_query=mindrift+onboarding+guide",
                    "AI Trainers Hub",
                    26),
                new VideoSeed(
                    "ابدأ مع Mindrift: دليل المهام الأولى",
                    GuideLanguage.Arabic,
                    "https://www.youtube.com/results?search_query=mindrift+arabic+tutorial",
                    "AI Trainers Hub Arabic",
                    18)
            },
            ["telus-digital"] = new[]
            {
                new VideoSeed(
                    "Telus Digital AI Community application tips",
                    GuideLanguage.English,
                    "https://www.youtube.com/results?search_query=telus+digital+ai+community+application",
                    "AI Trainers Hub",
                    24),
                new VideoSeed(
                    "التسجيل في مجتمع Telus Digital للذكاء الاصطناعي",
                    GuideLanguage.Arabic,
                    "https://www.youtube.com/results?search_query=telus+digital+arabic+community",
                    "AI Trainers Hub Arabic",
                    16)
            }
        };

        private static readonly OpportunitySeed[] OpportunitySeeds =
        {
            new(
                "Arabic RLHF Evaluator",
                "sara.hassan@example.com",
                "outlier",
                "Review Arabic prompts and rank model responses for helpfulness, accuracy, and cultural nuance. You will follow a detailed rubric, document your reasoning in English, and flag unsafe or hallucinated answers. Strong Modern Standard Arabic, 10–15 flexible hours per week, and reliable internet are required.",
                "راجع الأوامر العربية وقيّم استجابات النماذج من حيث الفائدة والدقة والملاءمة الثقافية. ستتبع معايير مفصلة وتوثق أسباب التقييم بالإنجليزية وتحدد الإجابات غير الآمنة أو المختلقة. يتطلب إتقان العربية الفصحى والتفرغ من 10 إلى 15 ساعة أسبوعيًا واتصالًا مستقرًا بالإنترنت.",
                "https://wa.me/201001234501?text=Arabic%20RLHF%20Evaluator",
                true,
                18),
            new(
                "Python Coding Trainer",
                "omar.khalid@example.com",
                "dataannotation",
                "Solve Python coding tasks and write clear reference solutions for an AI training dataset. You will explain edge cases, review generated code, and suggest tests. Solid Python fundamentals, Git experience, and at least 8 hours per week are required.",
                "حل مهام برمجة بلغة Python وكتابة حلول مرجعية واضحة لمجموعة بيانات تدريب الذكاء الاصطناعي. ستشرح الحالات الاستثنائية وتراجع الأكواد المولدة وتقترح اختبارات. يتطلب أساسيات قوية في Python وخبرة في Git والتفرغ 8 ساعات أسبوعيًا على الأقل.",
                "https://wa.me/201001234502?text=Python%20Coding%20Trainer",
                true,
                15),
            new(
                "STEM Reasoning Specialist",
                "sara.hassan@example.com",
                "mercor",
                "Create step-by-step solutions for math, physics, and logic problems used to evaluate frontier models. Strong written reasoning and the ability to catch subtle calculation errors are essential. A STEM degree or teaching experience is preferred.",
                "أنشئ حلولًا خطوة بخطوة لمسائل الرياضيات والفيزياء والمنطق المستخدمة في تقييم النماذج المتقدمة. يلزم استدلال كتابي قوي والقدرة على اكتشاف أخطاء الحساب الدقيقة، ويُفضل وجود شهادة في العلوم أو خبرة تدريسية.",
                "https://wa.me/201001234503?text=STEM%20Reasoning%20Specialist",
                true,
                12),
            new(
                "Arabic Localization Reviewer",
                "omar.khalid@example.com",
                "alignerr",
                "Review Arabic localizations of product interfaces and AI-generated copy. Check grammar, terminology, tone, and cultural fit, then leave actionable feedback for the localization team. Native Arabic fluency and professional English are required.",
                "راجع التعريب العربي لواجهات المنتجات والنصوص المولدة بالذكاء الاصطناعي. تحقق من القواعد والمصطلحات والنبرة والملاءمة الثقافية، وقدم ملاحظات قابلة للتنفيذ لفريق التعريب. يلزم إتقان العربية كلغة أم والإنجليزية بمستوى مهني.",
                "https://wa.me/201001234504?text=Arabic%20Localization%20Reviewer",
                true,
                10),
            new(
                "Speech Data Annotator (Arabic)",
                "sara.hassan@example.com",
                "telus-digital",
                "Transcribe and label short Arabic audio clips, including Egyptian and Gulf dialects. Mark speaker turns, background noise, and pronunciation issues using a structured annotation tool. Headphones and a quiet workspace are required.",
                "انسخ ووسم مقاطع صوتية عربية قصيرة، بما في ذلك اللهجات المصرية والخليجية. حدد أدوار المتحدثين والضوضاء الخلفية ومشكلات النطق باستخدام أداة وسم منظمة. يلزم استخدام سماعات ومساحة عمل هادئة.",
                "https://wa.me/201001234505?text=Speech%20Data%20Annotator",
                true,
                8),
            new(
                "AI Content Editor",
                "omar.khalid@example.com",
                "mindrift",
                "Edit AI-generated articles for clarity, factual consistency, and tone. You will rewrite weak sections, verify claims against provided sources, and apply the editorial style guide. Excellent written English and a sharp editorial eye are essential.",
                "حرر مقالات مولدة بالذكاء الاصطناعي من حيث الوضوح والاتساق الواقعي والنبرة. ستُعيد صياغة الأجزاء الضعيفة وتتحقق من الادعاءات مقابل المصادر المقدمة وتطبق دليل الأسلوب التحريري. يلزم إتقان الكتابة الإنجليزية وعين تحريرية دقيقة.",
                "https://wa.me/201001234506?text=AI%20Content%20Editor",
                true,
                6),
            new(
                "Mathematics Assessment Contributor",
                "sara.hassan@example.com",
                "mercor",
                "Design short mathematics assessments and grading rubrics for an AI evaluation suite. Topics include algebra, probability, and discrete math. A teaching background and experience writing assessment items are preferred.",
                "صمم اختبارات رياضيات قصيرة ومعايير تصحيح لمجموعة تقييم ذكاء اصطناعي. تشمل الموضوعات الجبر والاحتمالات والرياضيات المتقطعة، ويُفضل وجود خلفية تدريسية وخبرة في كتابة أسئلة التقييم.",
                "https://wa.me/201001234507?text=Mathematics%20Assessment",
                false,
                4),
            new(
                "Gulf Arabic Prompt Writer",
                "omar.khalid@example.com",
                "outlier",
                "Write natural Gulf Arabic prompts and reference answers for a conversational AI project. The role requires idiomatic fluency, attention to dialect differences, and 6–10 hours per week. Prior prompt-writing experience is a plus.",
                "اكتب أوامر عربية خليجية طبيعية وإجابات مرجعية لمشروع ذكاء اصطناعي حواري. يتطلب الدور طلاقة اصطلاحية واهتمامًا بفروق اللهجات والتفرغ من 6 إلى 10 ساعات أسبوعيًا، وتُعد خبرة كتابة الأوامر ميزة إضافية.",
                "https://wa.me/201001234508?text=Gulf%20Arabic%20Prompt%20Writer",
                false,
                2)
        };

        private static readonly ReviewSeed[] ReviewSeeds =
        {
            new(
                "outlier",
                "Sara Ahmed",
                5,
                "The onboarding was clear and payment arrived on time. Coding tasks are challenging but fair, and the reviewers explain what needs improvement.",
                true,
                45),
            new(
                "outlier",
                "Omar Khaled",
                4,
                "Good variety of reasoning projects and responsive support. Project availability changes from week to week, so flexibility helps.",
                true,
                38),
            new(
                "dataannotation",
                "Laila Mansour",
                5,
                "The assessment took time, but the instructions were precise and payment was reliable. I especially liked the writing and logic tasks.",
                true,
                35),
            new(
                "dataannotation",
                "Youssef Adel",
                4,
                "Steady work when projects are available. I would like to see more Arabic-language tasks and clearer expectations for retries.",
                true,
                28),
            new(
                "mercor",
                "Nour El-Sayed",
                5,
                "The interview process was professional and the rate matched the role. My first STEM project started within a week.",
                false,
                20),
            new(
                "mercor",
                "Karim Mostafa",
                4,
                "Great platform for STEM specialists. Communication could be faster during onboarding, but the project quality is excellent.",
                true,
                18),
            new(
                "alignerr",
                "Huda Al-Zahrani",
                5,
                "Excellent Arabic localization projects with helpful reviewer feedback. The guidelines are detailed and the work is interesting.",
                true,
                16),
            new(
                "alignerr",
                "Rania Benali",
                3,
                "Interesting tasks, but project availability fluctuates. The quality team is responsive when a task needs clarification.",
                true,
                14),
            new(
                "mindrift",
                "Ahmed Al-Masri",
                4,
                "Short missions fit around my studies and the platform is easy to use. Payments have been consistent so far.",
                true,
                12),
            new(
                "mindrift",
                "Mariam Hassan",
                5,
                "Clear guidelines and a friendly community. My first payout was smooth, and the Arabic writing missions are genuinely varied.",
                true,
                10),
            new(
                "telus-digital",
                "Daniel Weber",
                4,
                "Transcription work is well organized, though the application took a few weeks. The feedback loop is useful for improving accuracy.",
                false,
                8),
            new(
                "telus-digital",
                "Fatima Zahra",
                3,
                "Good experience overall, but some tasks were repetitive. The flexible schedule is a real advantage for students.",
                false,
                5)
        };

        private static readonly SuggestionSeed[] SuggestionSeeds =
        {
            new(
                "Prolific",
                "https://www.prolific.com/",
                "A research platform with frequent AI feedback and survey studies. It would be useful to add payout details, country availability, and typical study lengths to the directory.",
                "Hassan Ali",
                "General Insights",
                null,
                9),
            new(
                "Remotasks",
                "https://www.remotasks.com/",
                "منصة كبيرة لتعليقات البيانات ومهام الرؤية الحاسوبية والبرمجة. تختلف جودة التأهيل حسب المشروع، لذلك أرى أهمية إضافة تجارب المساهمين ومعلومات الدفع.",
                "Dina Farouk",
                "Coding & Development",
                null,
                6),
            new(
                "OneForma",
                "https://www.oneforma.com/",
                "Offers transcription, localization, and AI evaluation projects in Arabic. A good fit for bilingual contributors, with project-based onboarding and flexible hours.",
                "Yasmin Haddad",
                "Arabic Writing & Localization",
                null,
                3)
        };

        private static readonly AdvertisementSeed[] AdvertisementSeeds =
        {
            new(
                "Become a Certified AI Trainer",
                "Join a free 7-day bootcamp and learn how to pass platform assessments with confidence.",
                "Reserve your seat",
                "https://example.com/ai-trainer-bootcamp",
                "Hero"),
            new(
                "Get Paid to Review AI Responses",
                "Flexible remote work for bilingual experts across coding, writing, and reasoning projects.",
                "Explore roles",
                "https://example.com/remote-ai-roles",
                "Sidebar"),
            new(
                "Arabic AI Talent Program",
                "Premium opportunities for Arabic-speaking evaluators, localizers, and prompt writers.",
                "Apply now",
                "https://example.com/arabic-ai-talent",
                "BetweenCards")
        };

        public static async Task SeedAsync(
            AppDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            await EnsureDefaultAdminAsync(userManager);
            var leaders = await EnsureLeaderAsync(
                userManager,
                "Sara Hassan",
                "sara.hassan@example.com");
            var leaderTwo = await EnsureLeaderAsync(
                userManager,
                "Omar Khalid",
                "omar.khalid@example.com");

            var leadersByEmail = new Dictionary<string, ApplicationUser>(StringComparer.OrdinalIgnoreCase)
            {
                [leaders.Email!] = leaders,
                [leaderTwo.Email!] = leaderTwo
            };

            var categories = await EnsureCategoriesAsync(dbContext);
            var countries = await EnsureCountriesAsync(dbContext);
            var paymentMethods = await EnsurePaymentMethodsAsync(dbContext);

            var platformResult = await EnsurePlatformsAsync(dbContext);
            await ClearLegacyPlatformContentAsync(dbContext, platformResult.LegacyPlatformIds);
            await EnsurePlatformLinksAsync(
                dbContext,
                platformResult.Platforms,
                categories,
                countries,
                paymentMethods);
            await EnsureOnboardingStepsAsync(dbContext, platformResult.Platforms);
            await EnsureVideoGuidesAsync(dbContext, platformResult.Platforms);
            await EnsureWorkOpportunitiesAsync(
                dbContext,
                platformResult.Platforms,
                leadersByEmail);
            await EnsureReviewsAsync(dbContext, platformResult.Platforms);
            await EnsureSuggestionsAsync(dbContext, categories);
            await EnsureAdvertisementsAsync(dbContext);

            await dbContext.SaveChangesAsync();
        }

        private static async Task EnsureDefaultAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var admin = await userManager.FindByEmailAsync(DefaultAdminEmail);

            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    DisplayName = "Dev Mina",
                    UserName = DefaultAdminEmail,
                    Email = DefaultAdminEmail,
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-180)
                };

                var createResult = await userManager.CreateAsync(admin, DefaultAdminPassword);
                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Could not create the default admin: " +
                        string.Join(" ", createResult.Errors.Select(error => error.Description)));
                }
            }
            else if (!admin.IsActive)
            {
                admin.IsActive = true;
                await userManager.UpdateAsync(admin);
            }

            if (!await userManager.IsInRoleAsync(admin, Helpers.AdminRole))
            {
                await userManager.AddToRoleAsync(admin, Helpers.AdminRole);
            }
        }

        private static async Task<ApplicationUser> EnsureLeaderAsync(
            UserManager<ApplicationUser> userManager,
            string displayName,
            string email)
        {
            var leader = await userManager.FindByEmailAsync(email);

            if (leader is null)
            {
                leader = new ApplicationUser
                {
                    DisplayName = displayName,
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-90)
                };

                var createResult = await userManager.CreateAsync(leader, LeaderPassword);
                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not create the demo leader {email}: " +
                        string.Join(" ", createResult.Errors.Select(error => error.Description)));
                }
            }
            else
            {
                leader.DisplayName = displayName;
                leader.EmailConfirmed = true;
                leader.IsActive = true;
                await userManager.UpdateAsync(leader);
            }

            if (!await userManager.IsInRoleAsync(leader, Helpers.LeaderRole))
            {
                await userManager.AddToRoleAsync(leader, Helpers.LeaderRole);
            }

            return leader;
        }

        private static async Task<Dictionary<string, Category>> EnsureCategoriesAsync(AppDbContext dbContext)
        {
            var categories = await dbContext.Categories.ToListAsync();

            foreach (var seed in CategorySeeds)
            {
                var category = categories.FirstOrDefault(item =>
                    string.Equals(item.Name, seed.Name, StringComparison.OrdinalIgnoreCase));

                if (category is null)
                {
                    category = categories.FirstOrDefault(item =>
                        seed.LegacyNames.Any(legacy =>
                            string.Equals(item.Name, legacy, StringComparison.OrdinalIgnoreCase)));

                    if (category is null)
                    {
                        category = new Category(seed.Name, seed.Description);
                        dbContext.Categories.Add(category);
                        categories.Add(category);
                    }
                    else
                    {
                        category.Name = seed.Name;
                        category.Description = seed.Description;
                    }
                }
                else
                {
                    category.Description = seed.Description;
                }
            }

            await dbContext.SaveChangesAsync();

            return categories
                .GroupBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);
        }

        private static async Task<Dictionary<string, Country>> EnsureCountriesAsync(AppDbContext dbContext)
        {
            var countries = await dbContext.Countries.ToListAsync();

            foreach (var seed in CountrySeeds)
            {
                var country = countries.FirstOrDefault(item =>
                    string.Equals(item.IsoCode, seed.IsoCode, StringComparison.OrdinalIgnoreCase));

                if (country is null)
                {
                    country = new Country(seed.NameAr, seed.NameEn, seed.IsoCode);
                    dbContext.Countries.Add(country);
                    countries.Add(country);
                }
                else
                {
                    country.NameEn = seed.NameEn;
                    country.NameAr = seed.NameAr;
                }
            }

            await dbContext.SaveChangesAsync();

            return countries
                .GroupBy(item => item.IsoCode, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);
        }

        private static async Task<Dictionary<string, PaymentMethod>> EnsurePaymentMethodsAsync(AppDbContext dbContext)
        {
            var methods = await dbContext.PaymentMethods.ToListAsync();

            foreach (var seed in PaymentMethodSeeds)
            {
                var method = methods.FirstOrDefault(item =>
                    string.Equals(item.Name, seed.Name, StringComparison.OrdinalIgnoreCase));

                if (method is null)
                {
                    method = methods.FirstOrDefault(item =>
                        seed.LegacyNames.Any(legacy =>
                            string.Equals(item.Name, legacy, StringComparison.OrdinalIgnoreCase)));

                    if (method is null)
                    {
                        method = new PaymentMethod(seed.Name);
                        dbContext.PaymentMethods.Add(method);
                        methods.Add(method);
                    }
                    else
                    {
                        method.Name = seed.Name;
                    }
                }
            }

            await dbContext.SaveChangesAsync();

            return methods
                .GroupBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);
        }

        private sealed record PlatformSeedResult(
            Dictionary<string, Platform> Platforms,
            List<Guid> LegacyPlatformIds);

        private static async Task<PlatformSeedResult> EnsurePlatformsAsync(AppDbContext dbContext)
        {
            var platforms = await dbContext.Platforms.ToListAsync();
            var legacyPlatformIds = new List<Guid>();

            foreach (var seed in PlatformSeeds)
            {
                var platform = platforms.FirstOrDefault(item =>
                    string.Equals(item.Slug, seed.Slug, StringComparison.OrdinalIgnoreCase));

                platform ??= platforms.FirstOrDefault(item =>
                    string.Equals(item.Name, seed.Name, StringComparison.OrdinalIgnoreCase));

                if (platform is null && string.Equals(seed.Slug, "outlier", StringComparison.OrdinalIgnoreCase))
                {
                    platform = platforms.FirstOrDefault(item =>
                        string.Equals(item.Slug, "test-platform", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(item.Name, "Test Platform", StringComparison.OrdinalIgnoreCase));
                }

                if (platform is null)
                {
                    platform = new Platform(
                        seed.Name,
                        seed.Slug,
                        seed.DescriptionAr,
                        seed.DescriptionEn,
                        seed.WebsiteUrl,
                        null,
                        seed.Status)
                    {
                        CreatedAt = DateTime.UtcNow.AddDays(-seed.CreatedDaysAgo)
                    };

                    dbContext.Platforms.Add(platform);
                    platforms.Add(platform);
                }
                else
                {
                    if (string.Equals(platform.Slug, "test-platform", StringComparison.OrdinalIgnoreCase))
                    {
                        legacyPlatformIds.Add(platform.Id);
                    }

                    platform.Name = seed.Name;
                    platform.Slug = seed.Slug;
                    platform.DescriptionAr = seed.DescriptionAr;
                    platform.DescriptionEn = seed.DescriptionEn;
                    platform.WebsiteUrl = seed.WebsiteUrl;
                    platform.Status = seed.Status;
                }
            }

            await dbContext.SaveChangesAsync();

            var seededPlatforms = platforms
                .Where(item => PlatformSeeds.Any(seed =>
                    string.Equals(seed.Slug, item.Slug, StringComparison.OrdinalIgnoreCase)))
                .GroupBy(item => item.Slug, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);

            return new PlatformSeedResult(seededPlatforms, legacyPlatformIds);
        }

        private static async Task ClearLegacyPlatformContentAsync(
            AppDbContext dbContext,
            List<Guid> legacyPlatformIds)
        {
            if (legacyPlatformIds.Count == 0)
            {
                return;
            }

            foreach (var platformId in legacyPlatformIds)
            {
                await dbContext.OnboardingSteps
                    .Where(item => item.PlatformId == platformId)
                    .ExecuteDeleteAsync();
                await dbContext.VideoGuides
                    .Where(item => item.PlatformId == platformId)
                    .ExecuteDeleteAsync();
                await dbContext.Reviews
                    .Where(item => item.PlatformId == platformId)
                    .ExecuteDeleteAsync();
                await dbContext.WorkOpportunities
                    .Where(item => item.PlatformId == platformId)
                    .ExecuteDeleteAsync();
                await dbContext.PlatformCategories
                    .Where(item => item.PlatformId == platformId)
                    .ExecuteDeleteAsync();
                await dbContext.PlatformCountries
                    .Where(item => item.PlatformId == platformId)
                    .ExecuteDeleteAsync();
                await dbContext.PlatformPaymentMethods
                    .Where(item => item.PlatformId == platformId)
                    .ExecuteDeleteAsync();
            }
        }

        private static async Task EnsurePlatformLinksAsync(
            AppDbContext dbContext,
            Dictionary<string, Platform> platforms,
            Dictionary<string, Category> categories,
            Dictionary<string, Country> countries,
            Dictionary<string, PaymentMethod> paymentMethods)
        {
            foreach (var seed in PlatformSeeds)
            {
                if (!platforms.TryGetValue(seed.Slug, out var platform))
                {
                    continue;
                }

                var desiredCategoryIds = seed.Categories
                    .Select(name => categories[name].Id)
                    .ToHashSet();

                var categoryLinks = await dbContext.PlatformCategories
                    .Where(item => item.PlatformId == platform.Id)
                    .ToListAsync();

                foreach (var categoryId in desiredCategoryIds)
                {
                    if (categoryLinks.All(item => item.CategoryId != categoryId))
                    {
                        dbContext.PlatformCategories.Add(new PlatformCategory(platform.Id, categoryId));
                    }
                }

                dbContext.PlatformCategories.RemoveRange(
                    categoryLinks.Where(item => !desiredCategoryIds.Contains(item.CategoryId)));

                var countryLinks = await dbContext.PlatformCountries
                    .Where(item => item.PlatformId == platform.Id)
                    .ToListAsync();

                foreach (var countrySeed in seed.Countries)
                {
                    var country = countries[countrySeed.IsoCode];
                    var link = countryLinks.FirstOrDefault(item => item.CountryId == country.Id);

                    if (link is null)
                    {
                        dbContext.PlatformCountries.Add(new PlatformCountry(
                            platform.Id,
                            country.Id,
                            countrySeed.IsFullySupported,
                            countrySeed.Notes));
                    }
                    else
                    {
                        link.IsFullySupported = countrySeed.IsFullySupported;
                        link.Notes = countrySeed.Notes;
                    }
                }

                var desiredCountryIds = seed.Countries
                    .Select(item => countries[item.IsoCode].Id)
                    .ToHashSet();

                dbContext.PlatformCountries.RemoveRange(
                    countryLinks.Where(item => !desiredCountryIds.Contains(item.CountryId)));

                var paymentLinks = await dbContext.PlatformPaymentMethods
                    .Where(item => item.PlatformId == platform.Id)
                    .ToListAsync();

                var desiredPaymentIds = seed.PaymentMethods
                    .Select(name => paymentMethods[name].Id)
                    .ToHashSet();

                foreach (var paymentId in desiredPaymentIds)
                {
                    if (paymentLinks.All(item => item.PaymentMethodId != paymentId))
                    {
                        dbContext.PlatformPaymentMethods.Add(new PlatformPaymentMethod(platform.Id, paymentId));
                    }
                }

                dbContext.PlatformPaymentMethods.RemoveRange(
                    paymentLinks.Where(item => !desiredPaymentIds.Contains(item.PaymentMethodId)));
            }
        }

        private static async Task EnsureOnboardingStepsAsync(
            AppDbContext dbContext,
            Dictionary<string, Platform> platforms)
        {
            var steps = await dbContext.OnboardingSteps.ToListAsync();

            foreach (var seed in PlatformSeeds)
            {
                if (!platforms.TryGetValue(seed.Slug, out var platform))
                {
                    continue;
                }

                var platformSteps = steps
                    .Where(item => item.PlatformId == platform.Id)
                    .ToList();

                foreach (var template in OnboardingTemplates)
                {
                    var step = platformSteps.FirstOrDefault(item => item.StepNumber == template.StepNumber);

                    if (step is null)
                    {
                        step = new OnboardingStep(
                            platform.Id,
                            template.StepNumber,
                            template.TitleAr.Replace("{platform}", platform.Name),
                            template.TitleEn.Replace("{platform}", platform.Name),
                            template.DescriptionAr,
                            template.DescriptionEn,
                            template.Tip,
                            null);

                        dbContext.OnboardingSteps.Add(step);
                        platformSteps.Add(step);
                    }
                    else
                    {
                        step.TitleAr = template.TitleAr.Replace("{platform}", platform.Name);
                        step.TitleEn = template.TitleEn.Replace("{platform}", platform.Name);
                        step.DescriptionAr = template.DescriptionAr;
                        step.DescriptionEn = template.DescriptionEn;
                        step.Tip = template.Tip;
                    }
                }

                dbContext.OnboardingSteps.RemoveRange(
                    platformSteps.Where(item => item.StepNumber < 1 || item.StepNumber > OnboardingTemplates.Length));
            }
        }

        private static async Task EnsureVideoGuidesAsync(
            AppDbContext dbContext,
            Dictionary<string, Platform> platforms)
        {
            var guides = await dbContext.VideoGuides.ToListAsync();

            foreach (var platformSeed in PlatformSeeds)
            {
                if (!platforms.TryGetValue(platformSeed.Slug, out var platform) ||
                    !VideoSeeds.TryGetValue(platformSeed.Slug, out var videoSeeds))
                {
                    continue;
                }

                foreach (var videoSeed in videoSeeds)
                {
                    var guide = guides.FirstOrDefault(item =>
                        item.PlatformId == platform.Id &&
                        string.Equals(item.Url, videoSeed.Url, StringComparison.OrdinalIgnoreCase));

                    if (guide is null)
                    {
                        guide = new VideoGuide(
                            platform.Id,
                            videoSeed.Title,
                            videoSeed.Language,
                            videoSeed.Url,
                            "YouTube",
                            videoSeed.Author,
                            DateTime.UtcNow.AddDays(-videoSeed.PublishedDaysAgo))
                        {
                            IsApproved = true,
                            CreatedAt = DateTime.UtcNow.AddDays(-videoSeed.PublishedDaysAgo)
                        };

                        dbContext.VideoGuides.Add(guide);
                        guides.Add(guide);
                    }
                    else
                    {
                        guide.Title = videoSeed.Title;
                        guide.Language = videoSeed.Language;
                        guide.SourceType = "YouTube";
                        guide.AuthorOrChannel = videoSeed.Author;
                        guide.PublishedAt = DateTime.UtcNow.AddDays(-videoSeed.PublishedDaysAgo);
                        guide.IsApproved = true;
                    }
                }
            }
        }

        private static async Task EnsureWorkOpportunitiesAsync(
            AppDbContext dbContext,
            Dictionary<string, Platform> platforms,
            Dictionary<string, ApplicationUser> leaders)
        {
            var opportunities = await dbContext.WorkOpportunities.ToListAsync();

            foreach (var seed in OpportunitySeeds)
            {
                if (!platforms.TryGetValue(seed.PlatformSlug, out var platform) ||
                    !leaders.TryGetValue(seed.LeaderEmail, out var leader))
                {
                    continue;
                }

                var opportunity = opportunities.FirstOrDefault(item =>
                    item.LeaderId == leader.Id &&
                    item.PlatformId == platform.Id &&
                    string.Equals(item.Title, seed.Title, StringComparison.OrdinalIgnoreCase));

                if (opportunity is null)
                {
                    opportunity = new WorkOpportunity(
                        leader.Id,
                        platform.Id,
                        seed.Title,
                        seed.DescriptionAr,
                        seed.DescriptionEn,
                        seed.WhatsAppLink)
                    {
                        IsApproved = seed.IsApproved,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-seed.CreatedDaysAgo)
                    };

                    dbContext.WorkOpportunities.Add(opportunity);
                    opportunities.Add(opportunity);
                }
                else
                {
                    opportunity.DescriptionAr = seed.DescriptionAr;
                    opportunity.DescriptionEn = seed.DescriptionEn;
                    opportunity.WhatsAppLink = seed.WhatsAppLink;
                    opportunity.IsApproved = seed.IsApproved;
                    opportunity.IsActive = true;
                }
            }
        }

        private static async Task EnsureReviewsAsync(
            AppDbContext dbContext,
            Dictionary<string, Platform> platforms)
        {
            var reviews = await dbContext.Reviews.ToListAsync();

            foreach (var seed in ReviewSeeds)
            {
                if (!platforms.TryGetValue(seed.PlatformSlug, out var platform))
                {
                    continue;
                }

                var review = reviews.FirstOrDefault(item =>
                    item.PlatformId == platform.Id &&
                    string.Equals(item.ReviewerName, seed.ReviewerName, StringComparison.OrdinalIgnoreCase));

                if (review is null)
                {
                    review = new Review(platform.Id, seed.ReviewerName, seed.Rating, seed.Comment)
                    {
                        IsApproved = seed.IsApproved,
                        CreatedAt = DateTime.UtcNow.AddDays(-seed.CreatedDaysAgo)
                    };

                    dbContext.Reviews.Add(review);
                    reviews.Add(review);
                }
                else
                {
                    review.Rating = seed.Rating;
                    review.Comment = seed.Comment;
                    review.IsApproved = seed.IsApproved;
                }
            }
        }

        private static async Task EnsureSuggestionsAsync(
            AppDbContext dbContext,
            Dictionary<string, Category> categories)
        {
            var suggestions = await dbContext.PlatformSuggestions.ToListAsync();

            foreach (var seed in SuggestionSeeds)
            {
                var suggestion = suggestions.FirstOrDefault(item =>
                    string.Equals(item.PlatformName, seed.PlatformName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(item.SubmitterName, seed.SubmitterName, StringComparison.OrdinalIgnoreCase));

                var categoryId = categories.TryGetValue(seed.CategoryName, out var category)
                    ? category.Id
                    : (Guid?)null;

                if (suggestion is null)
                {
                    suggestion = new PlatformSuggestion(
                        seed.PlatformName,
                        seed.Url,
                        seed.Description,
                        seed.SubmitterName,
                        categoryId)
                    {
                        EmailOfUser = seed.Email,
                        IsReviewed = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-seed.CreatedDaysAgo)
                    };

                    dbContext.PlatformSuggestions.Add(suggestion);
                    suggestions.Add(suggestion);
                }
                else
                {
                    suggestion.Url = seed.Url;
                    suggestion.Description = seed.Description;
                    suggestion.CategoryId = categoryId;
                    suggestion.EmailOfUser = seed.Email;
                    suggestion.IsReviewed = false;
                }
            }
        }

        private static async Task EnsureAdvertisementsAsync(AppDbContext dbContext)
        {
            var advertisements = await dbContext.Advertisements.ToListAsync();
            var today = DateTime.UtcNow.Date;

            foreach (var seed in AdvertisementSeeds)
            {
                var advertisement = advertisements.FirstOrDefault(item =>
                    string.Equals(item.Position, seed.Position, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(item.Title, seed.Title, StringComparison.OrdinalIgnoreCase));

                if (advertisement is null)
                {
                    advertisement = new Advertisement(
                        seed.Title,
                        seed.Subtitle,
                        seed.CtaText,
                        seed.CtaUrl,
                        seed.Position,
                        true,
                        today.AddDays(-7),
                        today.AddDays(60))
                    {
                        CreatedAt = DateTime.UtcNow.AddDays(-7)
                    };

                    dbContext.Advertisements.Add(advertisement);
                    advertisements.Add(advertisement);
                }
                else
                {
                    advertisement.Subtitle = seed.Subtitle;
                    advertisement.CtaText = seed.CtaText;
                    advertisement.CtaUrl = seed.CtaUrl;
                    advertisement.IsActive = true;
                    advertisement.StartDate = today.AddDays(-7);
                    advertisement.EndDate = today.AddDays(60);
                }
            }
        }
    }
}
