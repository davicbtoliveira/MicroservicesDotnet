using System.Linq.Expressions;
using Common.Domain.Logic.Model;
using Common.Notification.Logic.Business.Intefaces;
using Common.Notification.Logic.Services;
using Common.UnitOfWork;
using Common.UnitOfWork.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Northwind.Data.Northwind.Context;
using Northwind.Data.Northwind.Entity;
using NorthwindService.Logic.Interfaces;
using NorthwindService.Logic.Validations;

namespace NorthwindService.Logic.Services
{
public class EmployeesService : BaseNotification, IEmployeesService
{
private readonly ILogger<EmployeesService> _logger;
private readonly INotification _notification;
private readonly IUnitOfWork<NorthwindContext> _unitOfWork;
private readonly IEmployeesService _employeesService;


public EmployeesService(ILogger<EmployeesService> logger,
                            INotification notificador,
                            IUnitOfWork<NorthwindContext> unitOfWork) : base(notificador)
{
    _logger = logger;
    _unitOfWork = unitOfWork;
}

public async Task<bool> AlterarAsync(Employees employees)
{
    try
    {
        if (!ExecutValidation(new EmployeesValidation(), employees)) return false;

        _unitOfWork.GetRepository<Employees>().Update(employees);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");

        throw;
    }
}

public async Task<bool> DeletarAsync(int employeeID)
{
    try
    {
        _unitOfWork.GetRepository<Employees>().Delete(employeeID);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");

        throw;
    }

}

public async Task<Employees> ObterAsync(int employeeID)
{
    try
    {
        return await _unitOfWork.GetRepository<Employees>().GetFirstOrDefaultAsync(predicate: x => x.EmployeeID == employeeID);
    }
    catch (Exception ex)
    {
        _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");

        throw;
    }
}

public async Task<IPagedList<Employees>> ObterPaginadoAsync(int? pageNo = 1, int? pageSize = 20)
{
    try
    {
        return await _unitOfWork.GetRepository<Employees>().GetPagedListAsync(pageIndex: pageNo.GetValueOrDefault(),
                                                                                            pageSize: pageSize.GetValueOrDefault());
    }
    catch (Exception ex)
    {
        _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
        throw;
    }
}

public async Task<IPagedList<Employees>> PostFiltroAsync(FiltroEmployeesDto filtroEmployeesDto)
{
    try
    {
        Expression<Func<Employees, bool>> criteria = null;

        if (filtroEmployeesDto.EmployeeID != null && filtroEmployeesDto.EmployeeID > 0)
            criteria = ExpressionExtensions.Combine(criteria, c => c.EmployeeID == filtroEmployeesDto.EmployeeID);

        if (!string.IsNullOrEmpty(filtroEmployeesDto.FirstName))
            criteria = ExpressionExtensions.Combine(criteria, c => c.FirstName.ToUpper().Contains(filtroEmployeesDto.FirstName.ToUpper()));

        if (!string.IsNullOrEmpty(filtroEmployeesDto.LastName))
            criteria = ExpressionExtensions.Combine(criteria, c => c.LastName.ToUpper().Contains(filtroEmployeesDto.LastName.ToUpper()));

        if (!string.IsNullOrEmpty(filtroEmployeesDto.Title))
            criteria = ExpressionExtensions.Combine(criteria, c => c.Title.ToUpper().Contains(filtroEmployeesDto.Title.ToUpper()));

        if (!string.IsNullOrEmpty(filtroEmployeesDto.TitleOfCourtesy))
            criteria = ExpressionExtensions.Combine(criteria, c => c.TitleOfCourtesy.ToUpper().Contains(filtroEmployeesDto.TitleOfCourtesy.ToUpper()));

        if (filtroEmployeesDto.BirthDate != null)
            criteria = ExpressionExtensions.Combine(criteria, c => c.BirthDate.Value.Date == filtroEmployeesDto.BirthDate);

        if (filtroEmployeesDto.HireDate != null)
            criteria = ExpressionExtensions.Combine(criteria, c => c.HireDate.Value.Date == filtroEmployeesDto.HireDate);

        if (!string.IsNullOrEmpty(filtroEmployeesDto.Address))
            criteria = ExpressionExtensions.Combine(criteria, c => c.Address.ToUpper().Contains(filtroEmployeesDto.Address.ToUpper()));

        if (!string.IsNullOrEmpty(filtroEmployeesDto.City))
            criteria = ExpressionExtensions.Combine(criteria, c => c.City.ToUpper().Contains(filtroEmployeesDto.City.ToUpper()));

        if (!string.IsNullOrEmpty(filtroEmployeesDto.PostalCode))
            criteria = ExpressionExtensions.Combine(criteria, c => c.PostalCode.ToUpper().Contains(filtroEmployeesDto.PostalCode.ToUpper()));

        if (!string.IsNullOrEmpty(filtroEmployeesDto.Country))
            criteria = ExpressionExtensions.Combine(criteria, c => c.Country.ToUpper().Contains(filtroEmployeesDto.Country.ToUpper()));

        if (!string.IsNullOrEmpty(filtroEmployeesDto.HomePhone))
            criteria = ExpressionExtensions.Combine(criteria, c => c.HomePhone.ToUpper().Contains(filtroEmployeesDto.HomePhone.ToUpper()));

        if (!string.IsNullOrEmpty(filtroEmployeesDto.Extension))
            criteria = ExpressionExtensions.Combine(criteria, c => c.Extension.ToUpper().Contains(filtroEmployeesDto.Extension.ToUpper()));

        if (!string.IsNullOrEmpty(filtroEmployeesDto.Notes))
            criteria = ExpressionExtensions.Combine(criteria, c => c.Notes.ToUpper().Contains(filtroEmployeesDto.Notes.ToUpper()));

        if (filtroEmployeesDto.ReportsTo != null)
            criteria = ExpressionExtensions.Combine(criteria, c => c.ReportsTo == filtroEmployeesDto.ReportsTo);

        if (!string.IsNullOrEmpty(filtroEmployeesDto.PhotoPath))
            criteria = ExpressionExtensions.Combine(criteria, c => c.PhotoPath.ToUpper().Contains(filtroEmployeesDto.PhotoPath.ToUpper()));


        if (filtroEmployeesDto.Situacao > 0)
            criteria = ExpressionExtensions.Combine(criteria, c => (int)filtroEmployeesDto.Situacao == c.Situacao.Value);

        return await _unitOfWork.GetRepository<Employees>()
            .GetPagedListAsync(
                predicate: criteria,
                pageIndex: filtroEmployeesDto.pageNo.Value,
                pageSize: filtroEmployeesDto.pageSize.Value,
                orderBy: (x) =>
                    filtroEmployeesDto.OrderByColumn == null
                        ? IQueryablePageListExtensions.OrderByColumnName(x, "EmployeeID", (bool)filtroEmployeesDto.IsAsc)
                        : IQueryablePageListExtensions.OrderByColumnName(x, filtroEmployeesDto.OrderByColumn, (bool)filtroEmployeesDto.IsAsc)
            );
    }
    catch (Exception ex)
    {
        _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
        throw;
    }
}

public async Task<bool> SalvarAsync(Employees employees)
{
    try
    {
        if (!ExecutValidation(new EmployeesValidation(), employees)) return false;

        await _unitOfWork.GetRepository<Employees>().InsertAsync(employees);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");

        throw;
    }
}
}
}

