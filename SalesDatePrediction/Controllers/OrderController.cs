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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;          
    }

        [HttpGet("next-orders")]
        public async Task<IActionResult> GetNextPredictedOrders([FromQuery] string? companyName)
        {
            try
            {
                var result = await _orderService.GetNextPredictedOrdersAsync(companyName);

                if (result.Data == null || !result.Data.Any())
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
                GenericResponse<List<ClientOrder>> result = await _orderService.GetClientOrdersAsync(customerId);

                if (result.Data == null || !result.Data.Any())
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

        [HttpPost]
        public async Task<ActionResult<GenericResponse<int>>> CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            try
            {
                GenericResponse<int>  reponse = await _orderService.CreateOrderAsync(request);
                return Ok(reponse);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio en CreateOrderAsync");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en CreateOrderAsync");
                return StatusCode(500, new { message = "Ocurrió un error interno. Inténtelo más tarde." });
            }
        }
    }
}
