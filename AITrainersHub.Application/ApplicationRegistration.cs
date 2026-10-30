using AITrainersHub.Application.PipelineBehavior;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AITrainersHub.Application
{
    public static class ApplicationRegistration 
    {
        public static void AddApplicationRegistration(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationRegistration).Assembly));
            services.AddValidatorsFromAssembly(typeof(ApplicationRegistration).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPipeline<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehavior<,>));

        }
    }
}
