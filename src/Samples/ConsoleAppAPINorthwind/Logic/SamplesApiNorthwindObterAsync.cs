using Common.Api.Logic.Models;
using Common.Domain.Logic.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Northwind.Data.Northwind.Entity;

namespace ConsoleAppAPINorthwind.Logic
{
    public class SamplesApiNorthwindObterAsync
    {
        public static async void Executar()
        {
            try
            {
                var serviceCollection = new ServiceCollection();

                IConfiguration Configuration = new ConfigurationBuilder()
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                             .AddEnvironmentVariables()
                             .Build();

                ConsoleAppAPINorthwind.Configuration.DependencyInjectionConfig.ResolveDependencies(serviceCollection, Configuration);

                var builder = serviceCollection.BuildServiceProvider();

                var httpClient = new HttpClient();
                
                var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8001/v1/Categories");
                request.Headers.Add("Usuario", "Usuario:wspereira");
                request.Headers.Add("Perfil", "Perfil:Administrador");
                
                var response = await httpClient.SendAsync(request);
                
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                    var proxyResult = JsonConvert.DeserializeObject<CustomResult<CategoriesApi>>(data);

                }


                //var categoriesService = builder.GetService<ICategoriesService>();

                //var categories = categoriesService.ObterAsync(3).Result;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;

                //Util.CategoriaToString(categories);

                Console.ReadKey();
                Console.WriteLine();
                Console.Clear();
                Console.WriteLine("\x1b[3J");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Average encerrado inesperadamente Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
                throw;
            }
        }


        public class CategoriesApi 
        {
            public int categoryID { get; set; }

            public string categoryName { get; set; }

            public string description { get; set; }

            public byte[] picture { get; set; }

        }
    }
}
