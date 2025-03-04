using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.UnitOfWork;
using ConsoleAppUnitOfWork.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Data.Northwind.Context;
using Northwind.Data.Northwind.Entity;

namespace ConsoleAppUnitOfWork.Logic.Repository
{
    public class SampleGetAll
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


                DependencyInjectionConfig.ResolveDependencies(serviceCollection, Configuration);

                var builder = serviceCollection.BuildServiceProvider();

                var _unitOfWork = builder.GetService<IUnitOfWork<NorthwindContext>>();

                var categories = _unitOfWork.GetRepository<Categories>().GetAll(disableTracking: false);

                foreach(var item in categories)
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
