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
    public class SampleDeletarAsync
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
                int categoryID = 0;
                bool validInput = false;

                while (!validInput)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Digite o ID da categoria que deseja deletar (ou digite 'cancel' para cancelar): ");
                    Console.ResetColor();
                    string input = Console.ReadLine();

                    if (input.Equals("cancel"))
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\nOperação cancelada pelo usuário.");
                        Console.ResetColor();
                        return;
                    }

                    validInput = int.TryParse(input, out categoryID);

                    if (!validInput)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Erro: O ID precisa ser um número inteiro. Tente novamente.");
                        Console.ResetColor();
                    }
                }


                var category = categoriesService.ObterAsync(categoryID).Result;

                if (category != null)
                {
                    categoriesService.DeletarAsync(category.CategoryID);

                    var defaultColor = Console.ForegroundColor;

                    Console.Write("\nRegistro de Nome ");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(category.CategoryName);

                    Console.ForegroundColor = defaultColor;
                    Console.Write(" deletado com sucesso");

                    Console.ResetColor();
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.ReadKey();
                    Console.WriteLine();
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.ReadKey();
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Categoria com Codigo {categoryID} não encontrada.");
                    Console.ResetColor();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Delete encerrado inesperadamente Exceção: {ex.GetType().FullName} | Mensagem: {ex.Message}");
                throw;
            }
        }

    }
}
