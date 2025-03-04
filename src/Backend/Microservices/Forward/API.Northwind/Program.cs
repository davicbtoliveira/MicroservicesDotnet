using API.Northwind.Configuration;
using Asp.Versioning.ApiExplorer;
using Common.Logging.Logic;
using Serilog;

try
{

    var builder = WebApplication.CreateBuilder(args);

    ConfigurationManager configuration = builder.Configuration;

    IWebHostEnvironment environment = builder.Environment;

    if (environment.IsDevelopment())
    {
        builder.AddSerilog(configuration["AppSettings:Description"].ToString());
        Log.Information("Starting " + configuration["AppSettings:Description"].ToString());
    }


    builder.Services.AddApiConfiguration(configuration);

    builder.Services.ResolveDependencies(configuration);

    builder.Services.AddSwaggerConfiguration(configuration);

    var app = builder.Build();

    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerConfiguration(environment, apiVersionDescriptionProvider, configuration);

    app.UseApiConfiguration();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host encerrado inesperadamente");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }


