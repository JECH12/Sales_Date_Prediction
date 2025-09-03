using Entities.Context;
using Entities.Models;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<AllEmployees>> GetAllEmployeesAsync()
        {
            try
            {
                return await _executor.ExecuteAsync<AllEmployees>("EXEC HR.GetEmployees");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando SP: HR.GetEmployees");
                throw new ApplicationException("Ocurrió un error al obtener los Empleados", ex);
            }

        }
    }
}
