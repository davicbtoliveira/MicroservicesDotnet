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
    public class SampleUpdate
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

                int categoryID = 5;

                var category = _unitOfWork.GetRepository<Categories>().GetFirstOrDefault(predicate: c => c.CategoryID == categoryID);

                var categoryName = category.CategoryName;

                if (category != null)
                {
                    category.CategoryName = "Costela Nova";
                    category.Description = "Costela Bovina Nova";

                    _unitOfWork.GetRepository<Categories>().Update(category);

                    _unitOfWork.SaveChanges();

                    var defaultColor = Console.ForegroundColor;

                    Console.Write("Registro de Nome ");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(categoryName);

                    Console.ForegroundColor = defaultColor;
                    Console.Write(" atualizado com nome ");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(category.CategoryName);

                    Console.ForegroundColor = defaultColor;
                    Console.Write(" e Descrição ");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(category.Description);

                    Console.ResetColor();
                    Console.WriteLine(" atualizado com sucesso");

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
                    Console.WriteLine($"Categoria com Nome {categoryName} não encontrada.");
                    Console.ResetColor();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"update encerrado inesperadamente Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
                throw;
            }
        }

    }
}
