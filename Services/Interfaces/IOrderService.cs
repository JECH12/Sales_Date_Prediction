using Entities.Models;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<GenericResponse<List<NextPreditedOrder>>> GetNextPredictedOrdersAsync(string? companyName);
        Task<GenericResponse<List<ClientOrder>>> GetClientOrdersAsync(int customerId);
        Task<GenericResponse<int>> CreateOrderAsync(CreateOrderRequestDto request);
    }
}
