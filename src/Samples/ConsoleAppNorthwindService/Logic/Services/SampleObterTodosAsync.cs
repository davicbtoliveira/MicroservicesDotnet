using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Data.Northwind.Context;
using Northwind.Data.Northwind.Entity;
using NorthwindService.Logic.Interfaces;

namespace ConsoleAppNorthwindService.Logic.Services
{
    public class SampleObterTodosAsync
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


                ConsoleAppNorthwindService.Configuration.DependencyInjectionConfig.ResolveDependencies(serviceCollection, Configuration);

                var builder = serviceCollection.BuildServiceProvider();

                var categoriesService = builder.GetService<ICategoriesService>();

                var categories = categoriesService.ObterTodosAsync().Result;
                foreach (var item in categories)
                {
                    Util.CategoriaToString(item);
                }

                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine();
                throw;
            }
        }

    }
}
