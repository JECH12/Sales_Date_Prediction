using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<NextPreditedOrder>> GetNextPredictedOrdersAsync();
        Task<List<ClientOrder>> GetClientOrdersAsync(int customerId);
    }
}
