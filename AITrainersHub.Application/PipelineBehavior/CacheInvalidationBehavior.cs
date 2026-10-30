using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.PipelineBehavior
{
    public class CacheInvalidationBehavior<TRequest, TResponse>(ICacheService cacheService)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1. تنفيذ الـ Command في قاعدة البيانات أولاً
            var response = await next();

            // 2. إذا كان الطلب يطبق واجهة مسح الكاش، امسح المفاتيح المحددة من Redis
            if (request is ICacheInvalidatorCommand invalidatorCommand)
            {
                foreach (var key in invalidatorCommand.CacheKeys)
                {
                    await cacheService.RemoveAsync(key, cancellationToken);
                }
            }

            return response;
        }
    }
}
