using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace SalesDatePrediction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _clientService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService clientService, ILogger<OrderController> logger)
        {
            _clientService = clientService;
            _logger = logger;          
    }

        [HttpGet("next-orders")]
        public async Task<IActionResult> GetNextPredictedOrders()
        {
            try
            {
                var result = await _clientService.GetNextPredictedOrdersAsync();

                if (result == null || !result.Any())
                    return NotFound("No se encontraron predicciones de órdenes.");

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio en GetNextOrders");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en GetNextOrders");
                return StatusCode(500, new { message = "Ocurrió un error interno. Inténtelo más tarde." });
            }
        }

        [HttpGet("{customerId}/orders")]
        public async Task<IActionResult> GetClientOrders(int customerId)
        {
            try
            {
                var result = await _clientService.GetClientOrdersAsync(customerId);

                if (result == null || !result.Any())
                    return NotFound($"No se encontraron órdenes para el cliente {customerId}.");

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio en GetClientOrders");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en GetClientOrders");
                return StatusCode(500, new { message = "Ocurrió un error interno. Inténtelo más tarde." });
            }
        }
    }
}
