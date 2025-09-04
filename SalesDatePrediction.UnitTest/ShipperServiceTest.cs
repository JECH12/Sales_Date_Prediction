using Entities.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;
using Services.Services;
using System.Net;


namespace SalesDatePrediction.UnitTest
{
    public class ShipperServiceTest
    {
        private readonly Mock<IStoredProcedureExecutor> _executorMock;
        private readonly Mock<ILogger<AllShippers>> _loggerMock;
        private readonly ShipperService _service;

        public ShipperServiceTest()
        {
            _executorMock = new Mock<IStoredProcedureExecutor>();
            _loggerMock = new Mock<ILogger<AllShippers>>();
            _service = new ShipperService(_executorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            // Arrange
            var expected = new List<AllShippers>
        {
            new AllShippers { ShipperId = 1, CompanyName = "Servientrega" },
            new AllShippers { ShipperId = 2, CompanyName = "Interrapidisimo" }
        };

            _executorMock
                .Setup(e => e.ExecuteAsync<AllShippers>("EXEC Sales.GetShippers", It.IsAny<object[]>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllShippersAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expected.Count, result.Data.Count);
            Assert.Contains(result.Data, s => s.CompanyName == "Servientrega");
        }

        [Fact]
        public async Task GetAll_ExecutorFails()
        {
            // Arrange
            _executorMock
                .Setup(e => e.ExecuteAsync<AllShippers>("EXEC Sales.GetShippers", It.IsAny<object[]>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _service.GetAllShippersAsync());
            Assert.Equal("Ocurrió un error al obtener los transportistas", ex.Message);
        }
    }
}
