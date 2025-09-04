using Entities.Context;
using Entities.Models;
using Microsoft.Extensions.Logging;
using Services.DTO;
using Services.Interfaces;
using System.Net;

namespace Services.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IStoredProcedureExecutor _executor;
        private readonly ILogger<AllEmployees> _logger;


        public EmployeeService(IStoredProcedureExecutor executor, ILogger<AllEmployees> logger)
        {
            _executor = executor;
            _logger = logger;
        }

        public async Task<GenericResponse<List<AllEmployees>>> GetAllEmployeesAsync()
        {
            try
            {
                GenericResponse<List<AllEmployees>> response = new() 
                { 
                    Data = await _executor.ExecuteAsync<AllEmployees>("EXEC HR.GetEmployees"),
                    StatusCode = HttpStatusCode.OK,
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando SP: HR.GetEmployees");
                throw new ApplicationException("Ocurrió un error al obtener los Empleados", ex);
            }

        }
    }
}
