using Common.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Data.Northwind.Context;
using Northwind.Data.Northwind.Entity;

namespace ConsoleAppUnitOfWork.Logic.Repository
{
    public class SampleFind
{

    public static void Executar()
    {
        try
        {
            var serviceCollection = new ServiceCollection();

            IConfiguration Configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();

            ConsoleAppUnitOfWork.Configuration.DependencyInjectionConfig.ResolveDependencies(serviceCollection, Configuration);

            var builder = serviceCollection.BuildServiceProvider();

            var _unitOfWork = builder.GetService<IUnitOfWork<NorthwindContext>>();

            int categoryID = 1;

            var category = _unitOfWork.GetRepository<Categories>().Find(categoryID);

            if (category != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Categoria encontrada:");
                Console.ResetColor();

                var defaultColor = Console.ForegroundColor;

                Console.Write("ID: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(category.CategoryID);

                Console.ForegroundColor = defaultColor;
                Console.Write(", Nome: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(category.CategoryName);

                Console.ForegroundColor = defaultColor;
                Console.Write(", Descrição: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(category.Description);

                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Categoria com ID {categoryID} não foi encontrada.");
                Console.ResetColor();
            }

            Console.ReadKey();
            Console.WriteLine();
            Thread.Sleep(1000);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\x1b[3J");
            Console.ReadKey();
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Find encerrado inesperadamente. Exceção: {ex.GetType().FullName} | Mensagem: {ex.Message}");
            throw;
        }
    }

}
}
