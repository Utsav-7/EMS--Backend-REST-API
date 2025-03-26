using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace EMS_Backend_Project.EMS.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitMiddleware> _logger;
        private static readonly ConcurrentDictionary<string, (int Count, DateTime Expiry)> _requestCounts = new();

        private const int MAX_REQUESTS = 10; // Max requests allowed per minute
        private static readonly TimeSpan TIME_WINDOW = TimeSpan.FromMinutes(1);
        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (_requestCounts.TryGetValue(ipAddress, out var entry))
            {
                // If the time window has expired, reset the count
                if (entry.Expiry < DateTime.UtcNow)
                {
                    _requestCounts[ipAddress] = (1, DateTime.UtcNow.Add(TIME_WINDOW));
                }
                else
                {
                    // If the limit is exceeded, return 429 Too Many Requests
                    if (entry.Count >= MAX_REQUESTS)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await httpContext.Response.WriteAsync("Too many requests. Please try again later.");
                        _logger.LogWarning($"Rate limit exceeded for IP: {ipAddress}");
                        return;
                    }

                    // Update request count
                    _requestCounts[ipAddress] = (entry.Count + 1, entry.Expiry);
                }
            }
            else
            {
                // Add new entry for the IP
                _requestCounts[ipAddress] = (1, DateTime.UtcNow.Add(TIME_WINDOW));
            }

            // Call the next middleware
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimitingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitMiddleware>();
        }
    }
}