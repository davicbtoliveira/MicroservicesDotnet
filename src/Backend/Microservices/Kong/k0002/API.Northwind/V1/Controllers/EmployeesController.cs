using System.Net;
using Asp.Versioning;
using AutoMapper;
using Common.Api.Logic.Controllers;
using Common.Domain.Logic.Model;
using Common.Notification.Logic.Business.Intefaces;
using Common.UnitOfWork.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data.Northwind.Context;
using Northwind.Data.Northwind.Entity;
using NorthwindService.Logic.Interfaces;

namespace API.Northwind.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("k0002/cotin-northwind/v{version:apiVersion}/[controller]")]
    public class EmployeesController : BaseApiController
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeesService _employeesService;

        public EmployeesController(ILogger<EmployeesController> logger,
                                    IEmployeesService employeesService,
                                    INotification notification,
                                    IMapper mapper,
                                    IHttpContextAccessor httpContextAccessor) : base(notification, httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;
            _employeesService = employeesService;
        }



        /// <summary>
        /// Método GET, destinado a busca de registros.
        /// </summary>
        /// <param name="id">Chave primária da tabela.</param>
        /// <returns>O envelope JSON correspondente ao resultado da pesquisa.</returns>

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeesDto>> GetEmployees(int id)
        {
            var employees = await _employeesService.ObterAsync(id);

            if (employees == null)
            {
                return NotFound();
            }

            var instanciaDto = _mapper.Map<EmployeesDto>(employees);

            return CustomResponse(instanciaDto);
        }



        /// <summary>
        /// Método GET, destinado a busca de registros, Categories com paginação.
        /// </summary>
        /// <param name="pageNo">Número da página que será exibida (default = 1).</param>
        /// <param name="pageSize">Quantidade de registros que serão exibidos (default = 20).</param>
        /// <returns>O envelope JSON correspondente ao resultado da pesquisa.</returns>

        // GET: api/Categories/All
        [HttpGet("ListagemEmployees")]
        [ProducesResponseType(typeof(IPagedList<EmployeesDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPaginado(int? pageNo = 1, int? pageSize = 20)
        {

            var instancia = await _employeesService.ObterPaginadoAsync(pageNo, pageSize);

            if (instancia == null) return NotFound();

            var instanciaDto = _mapper.Map<PagedList<EmployeesDto>>(instancia);

            return CustomResponse(instanciaDto);
        }

        /// <summary>
        /// Método POST, destinado a busca de registro por filtro.
        /// </summary>

        [HttpPost("Filtro")]
        [ProducesResponseType(typeof(PagedList<EmployeesDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> PostEmployeesFiltro([FromBody] FiltroEmployeesDto filtroDto)
        {
            var retorno = await _employeesService.PostFiltroAsync(filtroDto);
            return CustomResponse(retorno);
        }


        /// <summary>
        /// Método PUT, destinado a alteração de registros no banco.
        /// </summary>
        /// <param name="id">Instância em JSON do registro que será alterado.</param>
        /// <returns>O envelope JSON com registro alterado.</returns>

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutEmployees(EmployeesDto employees)
        {

            var instancia = await _employeesService.ObterAsync(employees.EmployeeID);

            if (instancia == null)
            {
                AddNotification($"Categories de ID {employees.EmployeeID} não encontrado.");

                return CustomResponse(false);
            }

            var instanciaDto = _mapper.Map<Employees>(employees);

            var atualiza = await _employeesService.AlterarAsync(instanciaDto);

            return CustomResponse(employees);

        }

        /// <summary>
        /// Método POST, destinado a criação de registros.
        /// </summary>
        /// <param name="Employees">Instância em JSON do registro para inclusão.</param>
        /// <returns>O envelope JSON do registro incluso com Chave Primária.</returns>

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostEmployees(EmployeesDto employees)
        {
            var instancia = await _employeesService.ObterAsync(employees.EmployeeID);

            if (instancia != null)
            {
                AddNotification($"Employees de ID {instancia.EmployeeID} já cadastrado.");

                return CustomResponse(false);
            }

            var instanciaDto = _mapper.Map<Employees>(employees);

            var atualiza = await _employeesService.SalvarAsync(instanciaDto);

            return CustomResponse(employees);

        }


        /// <summary>
        /// Método DELETE, destinado a exclusão de registros do banco, utilizando o ID.
        /// </summary>
        /// <param name="id">Chave primária da tabela.</param>
        /// <returns>O envelope JSON com registro excluído.</returns>

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployees(int id)
        {
            var instancia = await _employeesService.ObterAsync(id);

            if (instancia == null)
                AddNotification($"Employees de ID {id} não encontrado.");

            var atualiza = await _employeesService.DeletarAsync(id);

            return CustomResponse(atualiza);
        }

    }
}
