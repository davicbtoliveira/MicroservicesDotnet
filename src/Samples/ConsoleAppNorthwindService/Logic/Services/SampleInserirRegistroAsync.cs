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
    public class SampleInserirRegistroAsync
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


                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Digite o nome da categoria: ");
                Console.ResetColor();
                string categoryName = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Digite a descrição da categoria: ");
                Console.ResetColor();
                string description = Console.ReadLine();

                var categories = new Categories()
                {
                    CategoryName = categoryName,
                    Description = description
                };

                categoriesService.SalvarAsync(categories);

                var defaultColor = Console.ForegroundColor;

                Console.Write("\nRegistro Id de nome ");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(categories.CategoryName);

                Console.ForegroundColor = defaultColor;
                Console.Write(" e Descrição ");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(categories.Description);

                Console.ResetColor();
                Console.WriteLine(" inserido com sucesso");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.ReadKey();
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.ReadKey();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Insert encerrado inesperadamente Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
                throw;
            }
        }

    }
}
