using AspNetCoreRateLimit;

namespace CalendarGroupMeeting.Api.Infra.DI;

public static class Security
{
    public static void AddSecurity(this IServiceCollection services)
    {
        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = 429;
            options.RealIpHeader = "X-Real-IP";
            options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "1m",
            Limit = 60
        },
        new RateLimitRule
        {
            Endpoint = "POST:/events",
            Period = "1h",
            Limit = 10
        },
        new RateLimitRule
        {
            Endpoint = "POST:/responses",
            Period = "1m",
            Limit = 10
        }
    };
        });

        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddInMemoryRateLimiting();
    }
}
