using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class OrderDto
    {
        public int CustId { get; set; }
        public int EmpId { get; set; }
        public int ShipperId { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public string ShipAddress { get; set; } = string.Empty;
        public string ShipCity { get; set; } = string.Empty;
        public decimal Freight { get; set; }
        public string ShipCountry { get; set; } = string.Empty;
    }
}
