using Entities.Models;
using Microsoft.Extensions.Logging;
using Services.DTO;
using Services.Interfaces;
using System.Net;
namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IStoredProcedureExecutor _executor;

        private readonly ILogger<AllProducts> _logger;

        public ProductService(IStoredProcedureExecutor executor, ILogger<AllProducts> logger)
        {
            _executor = executor;
            _logger = logger;
        }

        public async Task<GenericResponse<List<AllProducts>>> GetAllProductsAsync()
        {
            try
            {
                GenericResponse<List<AllProducts>> response = new()
                {
                    Data = await _executor.ExecuteAsync<AllProducts>("EXEC Production.GetProducts"),
                    StatusCode = HttpStatusCode.OK
                };
                return response;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error ejecutando SP: Production.GetProducts");
                throw new ApplicationException("Ocurrió un error al obtener los productos", ex);
            }
        }
    }
}
