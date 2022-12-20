using Microsoft.AspNetCore.Mvc;
using Produtos.Domain.Model;
using Produtos.Domain.Model.Interfaces.ApplicationServices;
using Produtos.Domain.Model.ViewModels.Products;

namespace Produtos.WebApi.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : BaseController
    {
        private readonly IProductApplicationService _productApplicationService;

        public ProductsController(IProductApplicationService productApplicationService)
        {
            _productApplicationService = productApplicationService;
        }

        /// <summary>
        /// Get product by Id
        /// </summary>
        /// Example:
        ///
        /// [GET] products/146
        ///
        /// </remarks>
        /// <param name="id" example="146">ProductId</param>
        /// <response code="200">Returns success product result</response>
        /// <response code="400">Returns BadRequest</response>
        /// <response code="404">Returns NotFound</response>
        [ProducesResponseType(typeof(ProductResponseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseViewModel>> GetById(int id)
        {
            var result = await _productApplicationService.GetById(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get product by filter
        /// </summary>
        /// <remarks>
        /// Example:
        ///
        /// [GET] products:paginated?Id=1
        ///
        /// According to the necessary parameters
        /// </remarks>
        /// <response code="200">Returns list of products result</response>
        /// <response code="400">Returns BadRequest</response>
        /// <response code="404">Returns NotFound</response>
        [ProducesResponseType(typeof(PaginatedProductResponseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(":paginated")]
        public async Task<ActionResult<PaginatedProductResponseViewModel>> GetByFilter([FromQuery] GetProductsByFilter filter)
        {
            if (!ModelState.IsValid)
                return ProcessResponse(ServiceResult.BadRequestByModelState(ModelState));

            var result = await _productApplicationService.GetByFilter(filter);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Register new product
        /// </summary>
        /// <remarks>
        /// <response code="201">Returns in success case</response>
        /// <response code="400">Returns BadRequest</response>
        /// <response code="404">Returns NotFound</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<ActionResult<ProductResponseViewModel>> Register([FromBody] RegisterProductViewModel registerProductViewModel)
        {
            var result = await _productApplicationService.Register(registerProductViewModel);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update existing product
        /// </summary>
        /// <param name="id" example="146">ProductId</param>
        /// <response code="200">Returns in success case</response>
        /// <response code="400">Returns BadRequest</response>
        /// <response code="404">Returns NotFound</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RegisterProductViewModel registerProductViewModel)
        {
            var result = await _productApplicationService.Edit(id, registerProductViewModel);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Delete existing product
        /// </summary>
        /// <remarks>
        /// Example:
        ///
        /// [DELETE] products/1
        ///
        /// </remarks>
        /// <param name="id" example="146">ProductId</param>
        /// <response code="200">Returns in success case</response>
        /// <response code="400">Returns BadRequest</response>
        /// <response code="404">Returns NotFound</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productApplicationService.Delete(id);
            return ProcessResponse(result);
        }
    }
}
