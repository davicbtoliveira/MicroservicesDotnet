using EntityFrameworkCore.UseRowNumberForPaging;
using Microsoft.EntityFrameworkCore;
using Northwind.Data.Northwind.Context;
using Common.UnitOfWork;
using Common.Notification.Logic.Business.Intefaces;
using Common.Notification.Logic.Business.Notification;
using NorthwindService.Logic.Interfaces;
using NorthwindService.Logic.Services;
using System.Reflection;
using Common.Logging.Logic.Interface;
using Common.Logging.Logic;

namespace API.Northwind.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {


            //services.AddDbContext<NorthwindContext>(options =>
            //{
            //    options.UseSqlServer(configuration.GetConnectionString("NorthwindConnection"), builder => builder.UseRowNumberForPaging());
            //}, ServiceLifetime.Scoped);

            services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(configuration.GetConnectionString("NorthwindConnection"),
                                                    providerOptions => { providerOptions.CommandTimeout(1500000); providerOptions.UseCompatibilityLevel(120); providerOptions.EnableRetryOnFailure(); }
                                                    ),
                                                    ServiceLifetime.Scoped);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddUnitOfWork<NorthwindContext>();
            services.AddScoped<INotification, Notification>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRequestResponseLoggerService, RequestResponseLoggerService>();
            services.AddScoped<IRequestResponseLogModelCreator, RequestResponseLogModelCreator>();

        }

    }
}
