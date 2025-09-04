using Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;
using Services.Services;
using System.Net;

namespace SalesDatePrediction.UnitTest
{
    public class EmployeeServiceTest
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Mock<IStoredProcedureExecutor> _executorMock;
        private readonly Mock<ILogger<AllEmployees>> _loggerMock;

        public EmployeeServiceTest()
        {
            var services = new ServiceCollection();

            _executorMock = new Mock<IStoredProcedureExecutor>();
            _loggerMock = new Mock<ILogger<AllEmployees>>();

            services.AddSingleton(_executorMock.Object);
            services.AddSingleton(_loggerMock.Object);
            services.AddTransient<EmployeeService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            // Arrange
            var expected = new List<AllEmployees>
        {
            new AllEmployees { EmpId = 1, FullName = "Esteban Carrillo" },
            new AllEmployees { EmpId = 2, FullName = "Cristian Sanchez" }
        };

            _executorMock.Setup(e => e.ExecuteAsync<AllEmployees>("EXEC HR.GetEmployees", It.IsAny<object[]>()))
                         .ReturnsAsync(expected);

            var service = _serviceProvider.GetRequiredService<EmployeeService>();

            // Act
            var result = await service.GetAllEmployeesAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal("Esteban Carrillo", result.Data[0].FullName);
        }

        [Fact]
        public async Task GetAll_ExecutorFails()
        {
            // Arrange
            _executorMock.Setup(e => e.ExecuteAsync<AllEmployees>("EXEC HR.GetEmployees", It.IsAny<object[]>()))
                         .ReturnsAsync(new List<AllEmployees>());

            var service = _serviceProvider.GetRequiredService<EmployeeService>();

            // Act
            var result = await service.GetAllEmployeesAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task CreateOrder_ProductNotFound()
        {
            // Arrange
            _executorMock.Setup(e => e.ExecuteAsync<AllEmployees>("EXEC HR.GetEmployees", It.IsAny<object[]>()))
                         .ThrowsAsync(new Exception("DB error"));

            var service = _serviceProvider.GetRequiredService<EmployeeService>();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.GetAllEmployeesAsync());
            Assert.Equal("Ocurrió un error al obtener los Empleados", ex.Message);

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error ejecutando SP: HR.GetEmployees")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
                ),
                Times.Once
            );
        }
    }
}
