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
        private readonly Mock<IStoredProcedureExecutor> _executorMock;
        private readonly Mock<ILogger<OrderService>> _loggerMock;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _executorMock = new Mock<IStoredProcedureExecutor>();
            _loggerMock = new Mock<ILogger<OrderService>>();
            _service = new OrderService(_executorMock.Object, _loggerMock.Object, null!);
        }

        private StoreSampleContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<StoreSampleContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new StoreSampleContext(options);
        }

        // -------------------------
        // Tests para CreateOrderAsync
        // ---

        [Fact]
        public async Task CreateOrder_ReturnsNewOrderId()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var product = TestData.CreateValidProduct(10, 99.5m);
            context.Products.Add(product);
            context.SaveChanges();

            var request = new CreateOrderRequestDto
            {
                Order = new OrderDto
                {
                    CustId = 1,
                    EmpId = 2,
                    ShipperId = 3,
                    ShipName = "Cliente X",
                    ShipAddress = "Calle 123",
                    ShipCity = "Bogotá",
                    Freight = 50,
                    ShipCountry = "CO"
                },
                OrderDetail = new OrderDetailDto
                {
                    ProductId = 10,
                    Qty = 5,
                    Discount = 0.1f
                }
            };

            var executorMock = new Mock<IStoredProcedureExecutor>();
            executorMock
                .Setup(e => e.ExecuteAsync<CreateOrderResult>(
                    It.IsAny<string>(),
                    It.IsAny<object[]>()))
                .ReturnsAsync(new List<CreateOrderResult>
                {
                new CreateOrderResult { NewOrderId = 1234 }
                });

            var loggerMock = new Mock<ILogger<OrderService>>();
            var service = new OrderService(executorMock.Object, loggerMock.Object, context);

            // Act
            var result = await service.CreateOrderAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(1234, result.Data);
        }

        [Fact]
        public async Task CreateOrder_ProductNotFound()
        {
            // Arrange
            using var context = CreateInMemoryContext(); 
            var request = new CreateOrderRequestDto
            {
                Order = new OrderDto { CustId = 1, EmpId = 2, ShipperId = 3 },
                OrderDetail = new OrderDetailDto { ProductId = 99, Qty = 1, Discount = 0 }
            };

            var executorMock = new Mock<IStoredProcedureExecutor>();
            var loggerMock = new Mock<ILogger<OrderService>>();
            var service = new OrderService(executorMock.Object, loggerMock.Object, context);

            // Act
            var result = await service.CreateOrderAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(0, result.Data);
        }

        [Fact]
        public async Task CreateOrder_ExecutorFails()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var product = TestData.CreateValidProduct(10, 99.5m);
            context.Products.Add(product);
            context.SaveChanges();

            var request = new CreateOrderRequestDto
            {
                Order = new OrderDto { CustId = 1, EmpId = 2, ShipperId = 3 },
                OrderDetail = new OrderDetailDto { ProductId = 10, Qty = 1, Discount = 0 }
            };

            var executorMock = new Mock<IStoredProcedureExecutor>();
            executorMock
                .Setup(e => e.ExecuteAsync<CreateOrderResult>(
                    It.IsAny<string>(),
                    It.IsAny<object[]>()))
                .ThrowsAsync(new Exception("DB Error"));

            var loggerMock = new Mock<ILogger<OrderService>>();
            var service = new OrderService(executorMock.Object, loggerMock.Object, context);

            // Act
            var result = await service.CreateOrderAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(0, result.Data);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        // -------------------------
        // Tests para GetNextPredictedOrdersAsync
        // ---

        [Fact]
        public async Task ReturnsData_WhenSuccess()
        {
            // Arrange
            var expectedOrders = new List<NextPreditedOrder>
            {
                new NextPreditedOrder { CustomerId = 1, CustomerName = "Cliente Test", LastOrderDate = DateTime.UtcNow }
            };

            _executorMock
                .Setup(e => e.ExecuteAsync<NextPreditedOrder>(
                    It.IsAny<string>(),
                    It.IsAny<object[]>()))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _service.GetNextPredictedOrdersAsync("Company Test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Single(result.Data);
            Assert.Equal("Cliente Test", result.Data.First().CustomerName);
        }

        [Fact]
        public async Task ThrowsException_WhenExecutorFails()
        {
            // Arrange
            _executorMock
                .Setup(e => e.ExecuteAsync<NextPreditedOrder>(
                    It.IsAny<string>(),
                    It.IsAny<object[]>()))
                .ThrowsAsync(new Exception("DB Error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(
                () => _service.GetNextPredictedOrdersAsync("Company Test"));

            Assert.Contains("Ocurrió un error al obtener las órdenes predichas", ex.Message);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Theory]
        [InlineData("CompanyA")]
        [InlineData("CompanyB")]
        [InlineData(null)]
        public async Task CallsExecutor_WithCompanyNames(string? companyName)
        {
            // Arrange
            var fakeOrders = new List<NextPreditedOrder>
            {
                new NextPreditedOrder { CustomerId = 99, CustomerName = "Luis", LastOrderDate = DateTime.UtcNow }
            };

            _executorMock
                .Setup(e => e.ExecuteAsync<NextPreditedOrder>(
                    It.IsAny<string>(),
                    It.IsAny<object[]>()))
                .ReturnsAsync(fakeOrders);

            // Act
            var result = await _service.GetNextPredictedOrdersAsync(companyName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotEmpty(result.Data);

            _executorMock.Verify(e =>
                e.ExecuteAsync<NextPreditedOrder>(
                    It.Is<string>(q => q.Contains("EXEC Sales.GetNextPredictedOrders")),
                    It.IsAny<object[]>()),
                Times.Once);
        }

        // -------------------------
        // Tests para GetClientOrdersAsync
        // -------------------------

        [Fact]
        public async Task ClientOrders_ReturnsData()
        {
            // Arrange
            var expectedOrders = new List<ClientOrder>
            {
                new ClientOrder { OrderId = 1, ShipName = "Cliente 1", ShipCity = "Bogotá" }
            };

            _executorMock
                .Setup(e => e.ExecuteAsync<ClientOrder>(
                    It.IsAny<string>(),
                    It.IsAny<object[]>()))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _service.GetClientOrdersAsync(123);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Single(result.Data);
            Assert.Equal("Cliente 1", result.Data.First().ShipName);
        }

        [Fact]
        public async Task ClientOrders_ThrowsException()
        {
            // Arrange
            _executorMock
                .Setup(e => e.ExecuteAsync<ClientOrder>(
                    It.IsAny<string>(),
                    It.IsAny<object[]>()))
                .ThrowsAsync(new Exception("DB Error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(
                () => _service.GetClientOrdersAsync(456));

            Assert.Contains("Ocurrió un error al obtener las órdenes del cliente", ex.Message);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999)]
        public async Task ClientOrders_CallsExecutor(int customerId)
        {
            // Arrange
            var fakeOrders = new List<ClientOrder>
            {
                new ClientOrder { OrderId = 42, ShipName = "Jorge", ShipCity = "Cali" }
            };

            _executorMock
                .Setup(e => e.ExecuteAsync<ClientOrder>(
                    It.IsAny<string>(),
                    It.IsAny<object[]>()))
                .ReturnsAsync(fakeOrders);

            // Act
            var result = await _service.GetClientOrdersAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotEmpty(result.Data);

            _executorMock.Verify(e =>
                e.ExecuteAsync<ClientOrder>(
                    It.Is<string>(q => q.Contains("EXEC Sales.GetClientOrders")),
                    It.IsAny<object[]>()),
                Times.Once);
        }
    }
}
