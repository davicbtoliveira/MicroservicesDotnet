using Common.Domain.Logic.Model;
using Common.UnitOfWork.Collections;
using Northwind.Data.Northwind.Entity;

namespace NorthwindService.Logic.Interfaces
{
    public interface IEmployeesService
    {
        Task<Employees> ObterAsync(int employeeID);
        Task<IPagedList<Employees>> ObterPaginadoAsync(int? pageNo = 1, int? pageSize = 20);
        Task<IPagedList<Employees>> PostFiltroAsync(FiltroEmployeesDto filtroEmployeesDto);
        Task<bool> DeletarAsync(int employeeID);
        Task<bool> SalvarAsync(Employees employees);
        Task<bool> AlterarAsync(Employees employees);
    }
}
