using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Common.Api.Logic.Extension;
using Microsoft.OpenApi.Models;

namespace API.Northwind.Configuration
{
public static class SwaggerConfig
{

    public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {

        var buildingVersion = Assembly.GetExecutingAssembly().GetName().Version;
        var descriptionAPI = configuration["AppSettings:Description"].ToString();


        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });


        var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

        services.AddSwaggerGen(c =>
        {

            var description = $@"Application with Swagger and API versioning - Version {buildingVersion} 
                                    </br></br>
                                    <strong>Atenção</strong></br> 
                                    Para realizar o consumo dos endpoints é necessário possuir credenciais.
                                    Por favor, entre em contato com o responsável, para obter as credenciais.";

            foreach (var descriptionApi in provider.ApiVersionDescriptions)
            {
                c.SwaggerDoc(descriptionApi.GroupName, new OpenApiInfo()
                {
                    Version = descriptionApi.ApiVersion.ToString(),
                    Title = $"{descriptionAPI} - OnContainers Cotin",
                    Description = description,
                    Contact = new OpenApiContact() { Name = "Davi Oliveira", Email = "dcbtoliveira@hotmail.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                });
            }
            c.OperationFilter<SwaggerDefaultValues>();
            c.AddXmlComments();
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.EnableAnnotations();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                    },
                    new string[] {}
                }
            });

        });

    }

    public static void UseSwaggerConfiguration(this IApplicationBuilder app,
                                                    IWebHostEnvironment env,
                                                    IApiVersionDescriptionProvider provider,
                                                    IConfiguration configuration)
    {

        var cssCotin = "https://cotin.templates.ms.gov.br/css/swagger-tokens.css";
        var appName = configuration["AppSettings:AppName"].ToString();
        var descriptionAPI = configuration["AppSettings:Description"].ToString();
        if (env.IsDevelopment())
        {

            app.UseSwagger();
            app.UseSwaggerUI(
            options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var version = string.Format("{0} {1}", descriptionAPI, description.GroupName);
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", version);
                    options.DocumentTitle = appName;
                }

                options.InjectStylesheet(cssCotin);
            });
        }
        else
        {
            app.UseSwagger(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.RouteTemplate = $"/k0002/cotin-northwind/{description.GroupName}/swagger/{description.GroupName}/swagger.json";
                    c.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                    });
                }
            });

            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var version = string.Format("API.Northwind {0}", description.GroupName);
                    c.SwaggerEndpoint($"/k0002/cotin-northwind/{description.GroupName}/swagger/{description.GroupName}/swagger.json", version);
                    c.RoutePrefix = $"k0002/cotin-northwind/{description.GroupName}/swagger";
                }

                c.InjectStylesheet(cssCotin);
            });
        }
    }

}
}
