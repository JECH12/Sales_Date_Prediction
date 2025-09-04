using Entities.Context;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.DTO;
using Services.Interfaces;
using System.Net;


namespace Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IStoredProcedureExecutor _executor;
        private readonly ILogger<OrderService> _logger;
        private readonly StoreSampleContext _context;

        public OrderService(IStoredProcedureExecutor executor,
                             ILogger<OrderService> logger,
                             StoreSampleContext context)
        {
            _executor = executor;
            _logger = logger;
            _context = context;

        }

        public async Task<GenericResponse<List<NextPreditedOrder>>> GetNextPredictedOrdersAsync(string? companyName)
        {
            try
            {
                GenericResponse<List<NextPreditedOrder>> response = new()
                {
                    Data = await _executor.ExecuteAsync<NextPreditedOrder>(
                    "EXEC Sales.GetNextPredictedOrders @CompanyName = {0}", companyName!),
                    StatusCode = HttpStatusCode.OK
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando SP: Sales.GetNextPredictedOrders");
                throw new ApplicationException("Ocurrió un error al obtener las órdenes predichas", ex);
            }
        }

        public async Task<GenericResponse<List<ClientOrder>>> GetClientOrdersAsync(int customerId)
        {
            try
            {
                GenericResponse<List<ClientOrder>> response = new()
                { 
                    StatusCode = HttpStatusCode.OK,
                    Data = await _executor.ExecuteAsync<ClientOrder>(
                    "EXEC Sales.GetClientOrders @CustomerId = {0}", customerId)

                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando SP: Sales.GetClientOrders para CustomerId {CustomerId}", customerId);
                throw new ApplicationException("Ocurrió un error al obtener las órdenes del cliente", ex);
            }
        }

        public async Task<GenericResponse<int>> CreateOrderAsync(CreateOrderRequestDto request)
        {
            try
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Productid == request.OrderDetail.ProductId);
                DateTime today = DateTime.Now;
                var result = await _executor.ExecuteAsync<CreateOrderResult>(
                    "EXEC Sales.CreateOrder @Empid = {0}, @Custid = {1}, @Shipperid = {2}, @Shipname = {3}, @Shipaddress = {4}, @Shipcity = {4}, " +
                    "@Orderdate = {6}, @Requireddate = {7}, @Shippeddate = {8}, @Freight = {9}, @Shipcountry = {10}, " +
                    "@Productid = {11}, @Unitprice = {2}, @Qty = {13}, @Discount = {14}",
                    request.Order.EmpId,
                    request.Order.CustId,
                    request.Order.ShipperId,
                    request.Order.ShipName,
                    request.Order.ShipAddress,
                    request.Order.ShipCity,
                    today,
                    today.AddDays(26),
                    today.AddDays(5),
                    request.Order.Freight,
                    request.Order.ShipCountry,
                    request.OrderDetail.ProductId,
                    product!.Unitprice,
                    request.OrderDetail.Qty,
                    request.OrderDetail.Discount
                );

                GenericResponse<int> objResponse = new GenericResponse<int>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = result.FirstOrDefault()?.NewOrderId ?? 0
                };
                
                return objResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando Sales.CreateOrder");
                return new GenericResponse<int>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = 0
                };
            }
        }
    }
}
