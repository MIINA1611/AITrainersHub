using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AITrainersHub.Application.Interfaces;
using MediatR;

namespace AITrainersHub.Application.PipelineBehavior
{
    public class CachingBehavior<TRequest, TResponse>(ICacheService cacheService)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1. إذا لم يكن الطلب من نوع ICacheableQuery، ادخل للـ Handler مباشرة
            if (request is not ICacheableQuery cacheableQuery)
            {
                return await next();
            }

            // 2. البحث في Redis أولاً
            var cachedResponse = await cacheService.GetAsync<TResponse>(
                cacheableQuery.CacheKey,
                cancellationToken);

            if (cachedResponse is not null)
            {
                return cachedResponse;
            }

            // 3. في حال عدم وجود البيانات في Redis، نفذ الـ Handler لجلبها من قاعدة البيانات
            var response = await next();

            // 4. حفظ النتيجة في Redis للطلبات القادمة
            if (response is not null)
            {
                await cacheService.SetAsync(
                    cacheableQuery.CacheKey,
                    response,
                    cacheableQuery.Expiration,
                    cancellationToken);
            }

            return response;
        }
    }
}
