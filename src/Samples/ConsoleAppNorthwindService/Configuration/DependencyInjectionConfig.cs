using Common.Notification.Logic.Business.Intefaces;
using Common.Notification.Logic.Business.Notification;
using Common.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Data.Northwind.Context;
using NorthwindService.Logic.Interfaces;
using NorthwindService.Logic.Services;

namespace ConsoleAppNorthwindService.Configuration
{
    public static class DependencyInjectionConfig
    {

        public static void ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NorthwindContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("NorthwindConnection"), o => o.UseCompatibilityLevel(120));
            }, ServiceLifetime.Transient);



            services.AddUnitOfWork<NorthwindContext>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<INotification, Notification>();
            services.AddLogging();
        }

    }
}
