using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ApplicationHealth.Infrastructure.Middlewares
{
    public class VulnerabilitiesGuard
    {
        private readonly RequestDelegate _next;

        public VulnerabilitiesGuard(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {
            //  HTTP security headers => web config te belirtildi
            context.Response.Headers.Add("Header-Name", "Header-Value");
            // Iframe leri engellemek için yapıldı. deger SAMEORIGIN olursa sadece kendi hostinginden iframe alıanbilr 
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            // cross-site scripting ataklarını engellemek için yapıldı. The value 1 means enabled and the mode of block will block the browser from rendering the page.
            context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
            // MIME-type sniffing is an attack where a hacker tries to exploit missing metadata on served files. The header can be added in middleware:
            // The value of nosniff will prevent primarily old browsers from MIME-sniffing.
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            // When you click a link on a website, the calling URL is automatically transferred to the linked site. Unless this is necessary, you should disable it using the Referrer-Policy header:
            // There are a lot of possible values for this header, like same-origin that will set the referrer as long as the user stays on the same website.
            context.Response.Headers.Add("Referrer-Policy", "no-referrer");
            // You are probably not using Flash. Right? Right!!? If not, you can disable the possibility of Flash making cross-site requests using the X-Permitted-Cross-Domain-Policies header
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
            // ihtiyac duymadığımız alanları istemeyiz
            //context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'"); // full engelemek için yukardaki alanlar gecerli 
            context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; usb 'none'");
            // The Content-Security-Policy header, is a HTTP response header much like the ones from the previous post. The header helps to prevent code injection attacks like cross-site scripting and clickjacking, by telling the browser which dynamic resources that are allowed to load.
            // context.Response.Headers.Add("Content-Security-Policy", "self'");

            await _next.Invoke(context);

        }

    }
}
