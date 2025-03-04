using ConsoleAppNorthwindService.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwindService.Logic.Interfaces;

public class SamplesObterAsync
{

    public static async void Executar()
    {
        try
        {
            var serviceCollection =  new ServiceCollection();

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
                Console.Write("Digite o ID da categoria que deseja consultar (ou digite 'cancel' para cancelar): ");
                Console.ResetColor();

                string idInput = Console.ReadLine();

                if (idInput.Equals("cancel"))
                {
                    Console.WriteLine("\nOperação cancelada pelo usuário.");
                    return;
                }

                if (int.TryParse(idInput, out categoryID) && categoryID > 0)
                {
                    validInput = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Erro: Digite um número válido para o ID.");
                    Console.ResetColor();
                }
            }

            var categories = categoriesService.ObterAsync(categoryID).Result;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            Util.CategoriaToString(categories);

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
}