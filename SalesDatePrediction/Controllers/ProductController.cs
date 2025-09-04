using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;
using Services.Services;

namespace SalesDatePrediction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("AllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                GenericResponse<List<AllProducts>> result = await _productService.GetAllProductsAsync();

                if (result.Data == null || !result.Data.Any())
                    return NotFound("No se encontraron los productos.");

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio en GetAllProductsAsync");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en GetAllProductsAsync");
                return StatusCode(500, new { message = "Ocurrió un error interno. Inténtelo más tarde." });
            }
        }
    }
}
