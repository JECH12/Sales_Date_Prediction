using Entities.Models;
using Microsoft.Extensions.Logging;
using Services.DTO;
using Services.Interfaces;
using System.Net;

namespace Services.Services
{
    public class ShipperService : IShipperService
    {
        private readonly IStoredProcedureExecutor _executor;

        private readonly ILogger<AllShippers> _logger;

        public ShipperService(IStoredProcedureExecutor executor, ILogger<AllShippers> logger)
        {
            _executor = executor;
            _logger = logger;
        }

        public async Task<GenericResponse<List<AllShippers>>> GetAllShippersAsync()
        {
            try
            {
                GenericResponse<List<AllShippers>> response = new()
                {
                    Data = await _executor.ExecuteAsync<AllShippers>("EXEC Sales.GetShippers"),
                    StatusCode = HttpStatusCode.OK
                };

                return response;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error ejecutando SP: Sales.Shippers");
                throw new ApplicationException("Ocurrió un error al obtener los transportistas", ex);
            }
        }
    }
}
