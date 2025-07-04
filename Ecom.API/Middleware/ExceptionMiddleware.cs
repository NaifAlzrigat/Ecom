using Ecom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _timeLimitWindow = TimeSpan.FromSeconds(30);
        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!IsAllowedRequest(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new ExceptionAPI((int)HttpStatusCode.TooManyRequests, "Too Many Requests please Try Again Later");

                    await context.Response.WriteAsJsonAsync(response);

                }
                await _next(context);

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _environment.IsDevelopment() ?
                    new ExceptionAPI((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new ExceptionAPI((int)HttpStatusCode.InternalServerError, ex.Message);

                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private bool IsAllowedRequest(HttpContext context)
        {

            var ip = context.Connection.RemoteIpAddress.ToString();

            var cashKey = $"Rate: {ip}";
            var now = DateTime.Now;

            var (timeStamp, count) = _memoryCache.GetOrCreate(cashKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _timeLimitWindow;
                return (timeStamp: now, count: 0);

            });

            if (now - timeStamp < _timeLimitWindow)
            {
                if (count > 8)
                {
                    return false;
                }
                _memoryCache.Set(cashKey, (timeStamp: now, count: count += 1), _timeLimitWindow);

            }
            else
            {
                _memoryCache.Set(cashKey, (timeStamp: now, count: count), _timeLimitWindow);

            }

            return true;


        }

        private void ApplySecurityHeaders(HttpContext context)
        {
            // Prevent XSS attacks
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

            // Disable MIME sniffing
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";

            // Prevent clickjacking
            context.Response.Headers["X-Frame-Options"] = "DENY";
        }
    }
}
