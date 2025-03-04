using Asp.Versioning;
using AutoMapper;
using Common.Api.Logic.Controllers;
using Common.Domain.Logic.Model;
using Common.Notification.Logic.Business.Intefaces;
using Common.UnitOfWork.Collections;
using Microsoft.AspNetCore.Mvc;
using Northwind.Data.Northwind.Context;
using Northwind.Data.Northwind.Entity;
using NorthwindService.Logic.Interfaces;
using System.Net;

namespace API.Northwind.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class CategoriesController : BaseApiController
    {
        private readonly NorthwindContext _context;
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ILogger<CategoriesController> logger,
                                    ICategoriesService categoriesService,
                                    INotification notification,
                                    IMapper mapper,
                                    IHttpContextAccessor httpContextAccessor) : base(notification, httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;
            _categoriesService = categoriesService;
        }

        /// <summary>
        /// Método GET, destinado a busca de todos os registros da tabela Categories.
        /// </summary>
        /// <returns>O envelope JSON correspondente ao resultado da pesquisa.</returns>

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriesDto>>> GetCategories()
        {
            var instancia = await _categoriesService.ObterTodosAsync();

            var instanciaDto = _mapper.Map<List<CategoriesDto>>(instancia);

            return CustomResponse(instanciaDto);

        }


        /// <summary>
        /// Método GET, destinado a busca de registros.
        /// </summary>
        /// <param name="id">Chave primária da tabela.</param>
        /// <returns>O envelope JSON correspondente ao resultado da pesquisa.</returns>

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriesDto>> GetCategories(int id)
        {
            var categories = await _categoriesService.ObterAsync(id);

            if (categories == null)
            {
                return NotFound();
            }

            var instanciaDto = _mapper.Map<CategoriesDto>(categories);

            return CustomResponse(instanciaDto);
        }

        /// <summary>
        /// Método GET, destinado a busca de registros, Categories com paginação.
        /// </summary>
        /// <param name="pageNo">Número da página que será exibida (default = 1).</param>
        /// <param name="pageSize">Quantidade de registros que serão exibidos (default = 20).</param>
        /// <returns>O envelope JSON correspondente ao resultado da pesquisa.</returns>

        // GET: api/Categories/All
        [HttpGet("ListagemCategories")]
        [ProducesResponseType(typeof(IPagedList<CategoriesDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterPaginado(int? pageNo = 1, int? pageSize = 20)
        {

            var instancia = await _categoriesService.ObterPaginadoAsync(pageNo, pageSize);

            if (instancia == null) return NotFound();

            var instanciaDto = _mapper.Map<PagedList<CategoriesDto>>(instancia);

            return CustomResponse(instanciaDto);
        }

        /// <summary>
        /// Método GET, destinado a busca de registros.
        /// </summary>
        /// <param name="categoryName">Chave primária da tabela.</param>
        /// <returns>O envelope JSON correspondente ao resultado da pesquisa.</returns>

        // GET: api/Categories/5
        [HttpGet("CategoryName")]
        public async Task<ActionResult<CategoriesDto>> GetCategoryName(string categoryName)
        {
            var instancia = await _categoriesService.ObterAsync(categoryName);

            if (instancia == null) return NotFound();

            //var instanciaDto = _mapper.Map<CategoriesDto>(instancia);
            var instanciaDto = _mapper.Map<IList<CategoriesDto>>(instancia);

            return CustomResponse(instanciaDto);
        }

        /// <summary>
        /// Método PUT, destinado a alteração de registros no banco.
        /// </summary>
        /// <param name="id">Instância em JSON do registro que será alterado.</param>
        /// <returns>O envelope JSON com registro alterado.</returns>

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategories(CategoriesDto categories)
        {

            var instancia = await _categoriesService.ObterAsync(categories.CategoryID);

            if (instancia == null)
            {
                AddNotification($"Categories de ID {categories.CategoryID} não encontrado.");

                return CustomResponse(false);
            }

            var instanciaDto = _mapper.Map<Categories>(categories);

            var atualiza = await _categoriesService.AlterarAsync(instanciaDto);

            return CustomResponse(categories);

        }

        /// <summary>
        /// Método POST, destinado a criação de registros.
        /// </summary>
        /// <param name="Categories">Instância em JSON do registro para inclusão.</param>
        /// <returns>O envelope JSON do registro incluso com Chave Primária.</returns>

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCategories(CategoriesDto categories)
        {
            var instancia = await _categoriesService.ObterAsync(categories.CategoryID);

            if (instancia != null)
            {
                AddNotification($"Categories de ID {instancia.CategoryID} já cadastrado.");

                return CustomResponse(false);
            }

            var instanciaDto = _mapper.Map<Categories>(categories);

            var atualiza = await _categoriesService.SalvarAsync(instanciaDto);

            return CustomResponse(categories);

        }

        /// <summary>
        /// Método DELETE, destinado a exclusão de registros do banco, utilizando o ID.
        /// </summary>
        /// <param name="id">Chave primária da tabela.</param>
        /// <returns>O envelope JSON com registro excluído.</returns>

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategories(int id)
        {
            var instancia = await _categoriesService.ObterAsync(id);

            if (instancia == null)
                AddNotification($"Categories de ID {id} não encontrado.");

            var atualiza = await _categoriesService.DeletarAsync(id);

            return CustomResponse(atualiza);
        }
    }
}
