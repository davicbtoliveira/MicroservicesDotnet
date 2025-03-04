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
    public class CategoriesService : BaseNotification, ICategoriesService
    {
        private readonly ILogger<CategoriesService> _logger;
        private readonly INotification _notification;
        private readonly IUnitOfWork<NorthwindContext> _unitOfWork;

        public CategoriesService(ILogger<CategoriesService> logger,
                                 INotification notificador,
                                 IUnitOfWork<NorthwindContext> unitOfWork) : base(notificador)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
     

        public async Task<Categories> ObterAsync(int categoriesID)
        {
            try
            {
                return await _unitOfWork.GetRepository<Categories>().GetFirstOrDefaultAsync(predicate: x => x.CategoryID == categoriesID);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");

                throw;
            }
        }

        public async Task<IList<Categories>> ObterAsync(string categoryName)
        {
            try
            {

                return await _unitOfWork.GetRepository<Categories>()
                    .GetAllAsync(predicate: x => EF.Functions.Collate(x.CategoryName.ToUpper(), "Latin1_General_CI_AI").Contains(categoryName.ToUpper()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");

                throw;
            }
        }

        public async Task<IPagedList<Categories>> ObterPaginadoAsync(int? pageNo = 1, int? pageSize = 20)
        {

            try
            {
                return await _unitOfWork.GetRepository<Categories>().GetPagedListAsync(pageIndex: pageNo.GetValueOrDefault(),
                                                                                                  pageSize: pageSize.GetValueOrDefault());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");
                throw;
            }

        }

        public async Task<Categories> ObterPorIdSqlAsync(int categoriesID)
        {
            try
            {
                var Sql = $"SELECT CategoryID, CategoryName, Description, null as Picture, null as Products FROM Categories WHERE CategoryID = {categoriesID}";

                var categories = _unitOfWork.ExecSQL<Categories>(Sql).FirstOrDefault();

                return categories;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");

                throw;
            }
        }

        public async Task<IList<Categories>> ObterTodosAsync()
        {
            try
            {
                return await _unitOfWork.GetRepository<Categories>().GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");

                throw;
            }
        }

        public async Task<bool> AlterarAsync(Categories categories)
        {
            try
            {
                if (!ExecutValidation(new CategoriesValidation(), categories)) return false;

                _unitOfWork.GetRepository<Categories>().Update(categories);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");

                throw;
            }
        }

        public async Task<bool> DeletarAsync(int categoriesID)
        {
            try
            {
                _unitOfWork.GetRepository<Categories>().Delete(categoriesID);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " + $"Mensagem: {ex.Message}");

                throw;
            }
        }

        public async Task<bool> SalvarAsync(Categories categories)
        {
            try
            {
                if (!ExecutValidation(new CategoriesValidation(), categories)) return false;

                await _unitOfWork.GetRepository<Categories>().InsertAsync(categories);
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
