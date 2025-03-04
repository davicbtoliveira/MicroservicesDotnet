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
    public class SampleObterPorNomeAsync
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

                // Solicitar ao usuário o nome da categoria para consulta
                string categoryName = string.Empty;
                bool validInput = false;

                while (!validInput)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Digite o nome da categoria que deseja consultar (ou digite 'cancel' para cancelar): ");
                    Console.ResetColor();

                    categoryName = Console.ReadLine();

                    
                    if (categoryName.Equals("cancel"))
                    {
                        Console.WriteLine("\nOperação cancelada pelo usuário.");
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(categoryName))
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Erro: O nome da categoria não pode ser vazio. Tente novamente.");
                        Console.ResetColor();
                    }
                }

                var categories = categoriesService.ObterAsync(categoryName).Result;

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
