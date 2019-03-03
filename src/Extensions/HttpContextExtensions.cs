using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace Utilities.Extensions
{
    public static class HttpContextExtensions
    {
        // From https://stackoverflow.com/questions/40530474/enable-antiforgery-token-with-asp-net-core-and-jquery
        public static string GetAntiforgeryToken(this HttpContext httpContext)
        {
            var antiforgery = (IAntiforgery)httpContext.RequestServices.GetService(typeof(IAntiforgery));
            var tokenSet = antiforgery.GetAndStoreTokens(httpContext);
            var requestToken = tokenSet.RequestToken;
            return requestToken;
        }
    }
}
