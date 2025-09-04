using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class CreateOrderRequestDto
    {
        public OrderDto Order { get; set; } = new();
        public OrderDetailDto OrderDetail { get; set; } = new();
    }
}
