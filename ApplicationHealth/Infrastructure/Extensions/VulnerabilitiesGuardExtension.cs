using ApplicationHealth.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ApplicationHealth.Infrastructure.Extensions
{
    static public class VulnerabilitiesGuardExtension
    {
        public static IApplicationBuilder UseVulnerabilitiesGuard(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<VulnerabilitiesGuard>();
        }
    }
}
