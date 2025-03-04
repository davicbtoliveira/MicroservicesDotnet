using Common.Notification.Logic.Business.Intefaces;
using Common.Notification.Logic.Business.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppAPINorthwind.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<INotification, Notification>();
            services.AddLogging();
            services.AddHttpClient();


        }

    }
}
