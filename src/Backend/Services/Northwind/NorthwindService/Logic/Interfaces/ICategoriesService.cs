using Common.UnitOfWork.Collections;
using Northwind.Data.Northwind.Entity;

namespace NorthwindService.Logic.Interfaces
{
    public interface ICategoriesService
    {
        Task<Categories> ObterAsync(int categoriesID);
        Task<IList<Categories>> ObterAsync(string categoryName);
        Task<Categories> ObterPorIdSqlAsync(int categoriesID);
        Task<IList<Categories>> ObterTodosAsync();
        Task<IPagedList<Categories>> ObterPaginadoAsync(int? pageNo = 1, int? pageSize = 20);
        Task<bool> DeletarAsync(int categoriesID);
        Task<bool> SalvarAsync(Categories categories);
        Task<bool> AlterarAsync(Categories categories);

    }
}
