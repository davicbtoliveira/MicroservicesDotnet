using Common.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Data.Northwind.Context;
using Northwind.Data.Northwind.Entity;

namespace ConsoleAppUnitOfWork.Logic.Repository
{
    public class SampleFromSql
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

            string sqlQuery = "SELECT * FROM Categories WHERE CategoryName LIKE @p0";

            object[] sqlParameters = new object[] { "%te%" };

            var categories = _unitOfWork.GetRepository<Categories>()
                .FromSql(sqlQuery, sqlParameters)
                .ToList();

            foreach (var item in categories)
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
            Console.WriteLine($"FromSql encerrado inesperadamente. Exceção: {ex.GetType().FullName} | Mensagem: {ex.Message}");
            throw;
        }
    }

}
}
