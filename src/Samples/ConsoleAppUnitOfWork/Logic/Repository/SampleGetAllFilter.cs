using Common.Domain.Logic.Model;
using Common.UnitOfWork;
using Common.UnitOfWork.Collections;
using Common.UnitOfWork.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Data.Northwind.Context;
using Northwind.Data.Northwind.Entity;
using static ConsoleAppUnitOfWork.Logic.Util;

namespace ConsoleAppUnitOfWork.Logic.Repository
{
    public class SampleGetAllFilter
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

                var categoriesFilterDto = new CategoriesFilterDto()
                {
                    CategoryID = 1,
                    CategoryName = "Teste",
                    Description = "Breads",
                };

                var filter = new Filter<Categories>()
                 .WithPredicate(sc => (categoriesFilterDto.CategoryID.HasValue && sc.CategoryID == categoriesFilterDto.CategoryID.GetValueOrDefault()) ||
                                      (!string.IsNullOrWhiteSpace(categoriesFilterDto.CategoryName) &&
                                      EF.Functions.Collate(sc.CategoryName, "Latin1_General_CI_AI").Contains(categoriesFilterDto.CategoryName)) ||
                                      (!string.IsNullOrWhiteSpace(categoriesFilterDto.Description) &&
                                      EF.Functions.Collate(sc.Description, "Latin1_General_CI_AI").Contains(categoriesFilterDto.Description)))
                  .WithDisableTracking(true)
                  .WithInclude(x => x.Include(x => x.Products))
                  .WithOrderBy(x => Ordenar(categoriesFilterDto, x));

                var categories = _unitOfWork.GetRepository<Categories>().GetAll(filter);

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
                Console.WriteLine($"GetAllFilter encerrado inesperadamente Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
                throw;
            }
        }

        public static IOrderedQueryable<Categories> Ordenar(CategoriesFilterDto filter, IQueryable<Categories> x)
        {
            if (string.IsNullOrEmpty(filter.OrderByColumn))
            {
                return x.OrderBy(x => x.CategoryID);
            }

            else
            {
                if (filter.OrderByColumn == "CategoryName")
                    return (filter.IsAsc == true) ?
                            x.OrderBy(x => x.CategoryName) :
                            x.OrderByDescending(x => x.CategoryName);

                else if (filter.OrderByColumn == "Description")
                    return (filter.IsAsc == true) ?
                            x.OrderBy(x => x.Description) :
                            x.OrderByDescending(x => x.Description);

                return x.OrderByColumnName(filter.OrderByColumn, filter.IsAsc);
            }
        }
    }
}
