using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Application.Abstractions.Services;

namespace Talabat.APIs.Controllers.Attributes
{
    internal class CachedAttribute(int minutes) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _responseCacheServices = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var CasheKey = GenerateCachKeyFromRequest(context.HttpContext.Request);
            var CachedResponse = await _responseCacheServices.GetCachedResponseAsync(CasheKey);

            if (!CachedResponse.IsNullOrEmpty())
            {
                var contentResult = new ContentResult()
                {
                    Content = CachedResponse,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                context.Result = contentResult;
                return;
            }

            var ExecutedEndPointContext = await next.Invoke();

            if (ExecutedEndPointContext.Result is OkObjectResult Result)
            {
               await _responseCacheServices.CacheResponseAsync(CasheKey, Result.Value,TimeSpan.FromMinutes(minutes));
            }

        }

        private string GenerateCachKeyFromRequest(HttpRequest request)
        {
            StringBuilder KeyBuilder = new StringBuilder();

            KeyBuilder.Append(request.Path); // api/products

            foreach (var (key, value) in request.Query.OrderBy(kvp => kvp.Key))
            {
                KeyBuilder.Append($"|{key}-{value}");
            }

            return KeyBuilder.ToString();
        }
    }
}
