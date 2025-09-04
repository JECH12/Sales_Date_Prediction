using Entities.Context;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Services.DTO;
using Services.Interfaces;
using Services.Services;
using System.Net;

namespace SalesDatePrediction.UnitTest
{
    public class OrderServiceTests
    {
        private readonly StoreSampleContext _mockContext;
        private readonly Mock<IStoredProcedureExecutor> _mockExecutor;
        private readonly Mock<ILogger<OrderService>> _mockLogger;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
             
            _mockExecutor = new Mock<IStoredProcedureExecutor>();
            _mockLogger = new Mock<ILogger<OrderService>>();
            _mockContext = CreateInMemoryContext();
            _orderService = new OrderService(
                _mockExecutor.Object,
                _mockLogger.Object,
                _mockContext
            );
        }

        private StoreSampleContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<StoreSampleContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new StoreSampleContext(options);

            // Poblar con datos iniciales que vas a necesitar en los tests
            context.Products.Add(new Product { Productid = 10, Unitprice = 50 });
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task CreateOrder_Success()
        {
            
            // Arrange
            var request = new CreateOrderRequestDto
            {
                Order = new OrderDto
                {
                    EmpId = 1,
                    ShipperId = 1,
                    ShipName = "Test Ship",
                    ShipAddress = "Test Address",
                    ShipCity = "Test City",
                    Freight = 100,
                    ShipCountry = "Test Country"
                },
                OrderDetail = new OrderDetailDto
                {
                    ProductId = 10,
                    Qty = 2,
                    Discount = 0
                }
            };

            var product = new Product { Productid = 10, Unitprice = 50 };

            _mockExecutor.Setup(e => e.ExecuteAsync<CreateOrderResult>(It.IsAny<string>(), It.IsAny<object[]>()))
                         .ReturnsAsync(new List<CreateOrderResult> { new CreateOrderResult { NewOrderId = 123 } });

            // Act
            var response = await _orderService.CreateOrderAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(123, response.Data);
        }

        [Fact]
        public async Task CreateOrder_ProductNotFound()
        {
            // Arrange
            var request = new CreateOrderRequestDto
            {
                Order = new OrderDto
                {
                    EmpId = 1,
                    ShipperId = 1,
                    ShipName = "Test Ship",
                    ShipAddress = "Test Address",
                    ShipCity = "Test City",
                    Freight = 100,
                    ShipCountry = "Test Country"
                },
                OrderDetail = new OrderDetailDto
                {
                    ProductId = 99,
                    Qty = 1,
                    Discount = 0
                }
            };

            // Act
            var response = await _orderService.CreateOrderAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(0, response.Data);
        }

        [Fact]
        public async Task CreateOrder_SPFailure()
        {
            // Arrange
            var request = new CreateOrderRequestDto
            {
                Order = new OrderDto
                {
                    EmpId = 1,
                    ShipperId = 1,
                    ShipName = "Test Ship",
                    ShipAddress = "Test Address",
                    ShipCity = "Test City",
                    Freight = 100,
                    ShipCountry = "Test Country"
                },
                OrderDetail = new OrderDetailDto
                {
                    ProductId = 10,
                    Qty = 1,
                    Discount = 0
                }
            };

            _mockExecutor.Setup(e => e.ExecuteAsync<CreateOrderResult>(It.IsAny<string>(), It.IsAny<object[]>()))
                         .ThrowsAsync(new Exception("DB error"));

            // Act
            var response = await _orderService.CreateOrderAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal(0, response.Data);
        }
    }
}
