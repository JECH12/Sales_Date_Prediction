using Entities.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SalesDatePrediction.UnitTest
{
    public class ProductServiceTest
    {
        private readonly Mock<IStoredProcedureExecutor> _executorMock;
        private readonly Mock<ILogger<AllProducts>> _loggerMock;
        private readonly ProductService _service;

        public ProductServiceTest()
        {
            _executorMock = new Mock<IStoredProcedureExecutor>();
            _loggerMock = new Mock<ILogger<AllProducts>>();
            _service = new ProductService(_executorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            // Arrange
            var expected = new List<AllProducts>
        {
            new AllProducts { ProductId = 1, ProductName = "PC", UnitPrice = 12 },
            new AllProducts { ProductId = 2, ProductName = "Mouse", UnitPrice = 25 }
        };

            _executorMock
                .Setup(e => e.ExecuteAsync<AllProducts>("EXEC Production.GetProducts", It.IsAny<object[]>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expected.Count, result.Data.Count);
            Assert.Contains(result.Data, p => p.ProductName == "PC");
        }

        [Fact]
        public async Task GetAll_ExecutorFails_Throws()
        {
            // Arrange
            _executorMock
                .Setup(e => e.ExecuteAsync<AllProducts>("EXEC Production.GetProducts", It.IsAny<object[]>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _service.GetAllProductsAsync());
            Assert.Equal("Ocurrió un error al obtener los productos", ex.Message);
        }
    }
}
