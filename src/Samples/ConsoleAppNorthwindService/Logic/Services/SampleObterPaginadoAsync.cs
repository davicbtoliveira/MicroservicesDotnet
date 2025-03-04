using ConsoleAppUnitOfWork.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwindService.Logic.Interfaces;

namespace ConsoleAppNorthwindService.Logic.Services
{
    public class SampleObterPaginadoAsync
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

                int pageNumber = 1;
                int pageSize = 3;

                bool validInput = false;

                while (!validInput)
                {
                    // Solicitar ao usuário qual página deseja visualizar
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Digite o número da página que deseja consultar (ou digite 'cancel' para cancelar): ");
                    Console.ResetColor();

                    string pageInput = Console.ReadLine();

                    
                    if (pageInput.Equals("cancel"))
                    {
                        Console.WriteLine("\nOperação cancelada pelo usuário.");
                        return;
                    }

                    if (int.TryParse(pageInput, out pageNumber) && pageNumber > 0)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Erro: Digite um número válido para a página.");
                        Console.ResetColor();
                    }
                }


                var categories = categoriesService.ObterPaginadoAsync(pageNumber, pageSize).Result;

                foreach (var item in categories.Items)
                {
                    Util.CategoriaToString(item);
                }

                Console.ReadKey();
                Console.WriteLine();
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.ReadKey();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"GetPagedListAsync encerrado inesperadamente Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
                throw;
            }
        }

    }
}
