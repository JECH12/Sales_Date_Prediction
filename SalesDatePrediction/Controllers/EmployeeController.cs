using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace SalesDatePrediction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet("AllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var result = await _employeeService.GetAllEmployeesAsync();

                if (result == null || !result.Any())
                    return NotFound("No se encontraron los empleados.");

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio en GetAllEmployees");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en GetAllEmployees");
                return StatusCode(500, new { message = "Ocurrió un error interno. Inténtelo más tarde." });
            }
        }
    }
}
