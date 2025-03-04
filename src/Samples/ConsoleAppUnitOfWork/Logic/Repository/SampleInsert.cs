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

namespace ConsoleAppUnitOfWork.Logic.Repository
{
    public class SampleInsert
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

                var categories = new Categories()
                {
                    CategoryName = "Costela",
                    Description = "Costela Bovina"
                };

                var inserir = _unitOfWork.GetRepository<Categories>().Insert(categories);

                _unitOfWork.SaveChanges();

                var defaultColor = Console.ForegroundColor;

                Console.Write("Registro Id de nome ");

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
