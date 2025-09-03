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
    public class ProductService : IProductService
    {
        private readonly IStoredProcedureExecutor _executor;

        private readonly ILogger<AllProducts> _logger;

        public ProductService(IStoredProcedureExecutor executor, ILogger<AllProducts> logger)
        {
            _executor = executor;
            _logger = logger;
        }

        public async Task<List<AllProducts>> GetAllProductsAsync()
        {
            try
            {
                return await _executor.ExecuteAsync<AllProducts>("EXEC Production.GetProducts");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error ejecutando SP: Production.GetProducts");
                throw new ApplicationException("Ocurrió un error al obtener los productos", ex);
            }
        }
    }
}
