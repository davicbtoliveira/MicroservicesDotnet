using Common.Api.Logic.Extension;
using Common.Logging.Logic;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace API.Northwind.Configuration
{
    public static class ApiConfig
    {

        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddOptions<RequestResponseLoggerOption>().Bind(configuration.GetSection("RequestResponseLogger")).ValidateDataAnnotations();

            services.AddMvc(options =>
            {
                options.Filters.Add(new RequestResponseLoggerActionFilter());
                options.Filters.Add(new RequestResponseLoggerErrorFilter());
            });
        }

        public static void UseApiConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMiddleware<RequestResponseLoggerMiddleware>();

            var cultureInfo = new CultureInfo("pt-BR");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var supportedCultures = new[] { cultureInfo };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

        }

    }
}
