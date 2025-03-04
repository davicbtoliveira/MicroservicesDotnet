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
    public class SampleGetFirstOrDefault
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

                var categories = _unitOfWork.GetRepository<Categories>().GetFirstOrDefault();

                if (categories != null)
                {
                    Util.CategoriaToString(categories);
                }

                else
                {
                    Console.WriteLine("Nenhuma categoria encontrada");
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
                Console.WriteLine($"GetFirstOrDefault encerrado inesperadamente Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
                throw;
            }
        }

    }
}
