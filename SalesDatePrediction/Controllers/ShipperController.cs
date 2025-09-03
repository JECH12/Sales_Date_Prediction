using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace SalesDatePrediction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;
        private readonly ILogger<ShipperController> _logger;

        public ShipperController(IShipperService shipperService, ILogger<ShipperController> logger)
        {
            _shipperService = shipperService;
            _logger = logger;
        }

        [HttpGet("AllShippers")]
        public async Task<IActionResult> GetAllShippers()
        {
            try
            {
                var result = await _shipperService.GetAllShippersAsync();

                if (result == null || !result.Any())
                    return NotFound("No se encontraron los transportistas.");

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio en GetAllShippersAsync");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en GetAllShippersAsync");
                return StatusCode(500, new { message = "Ocurrió un error interno. Inténtelo más tarde." });
            }
        }
    }
}
