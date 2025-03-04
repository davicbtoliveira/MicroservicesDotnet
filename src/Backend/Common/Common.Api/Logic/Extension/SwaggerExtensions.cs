using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Api.Logic.Extension
{
    public static class SwaggerExtensions
    {
        public static void AddXmlComments(this SwaggerGenOptions options)
        {
            var basePath = AppContext.BaseDirectory;
            var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            var fileName = Path.GetFileName(assemblyName + ".xml");
            var xmlDocumentPath = Path.Combine(basePath, fileName);

            if (File.Exists(xmlDocumentPath))
            {
                options.IncludeXmlComments(xmlDocumentPath);
            };

        }
    }
}
