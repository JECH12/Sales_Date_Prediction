using Entities.Models;
using Microsoft.Extensions.Logging;
using Services.Interfaces;


namespace Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IStoredProcedureExecutor _executor;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IStoredProcedureExecutor executor,
                             ILogger<OrderService> logger)
        {
            _executor = executor;
            _logger = logger;

        }

        public async Task<List<NextPreditedOrder>> GetNextPredictedOrdersAsync()
        {
            try
            {
                return await _executor.ExecuteAsync<NextPreditedOrder>(
                    "EXEC Sales.GetNextPredictedOrders"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando SP: Sales.GetNextPredictedOrders");
                throw new ApplicationException("Ocurrió un error al obtener las órdenes predichas", ex);
            }
        }

        public async Task<List<ClientOrder>> GetClientOrdersAsync(int customerId)
        {
            try
            {
                return await _executor.ExecuteAsync<ClientOrder>(
                    "EXEC Sales.GetClientOrders @CustomerId = {0}", customerId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando SP: Sales.GetClientOrders para CustomerId {CustomerId}", customerId);
                throw new ApplicationException("Ocurrió un error al obtener las órdenes del cliente", ex);
            }
        }
    }
}
