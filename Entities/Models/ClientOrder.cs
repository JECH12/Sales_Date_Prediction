using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ClientOrder
    {
        public int OrderId { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public string ShipAddress { get; set; } = string.Empty;
        public string ShipCity { get; set; } = string.Empty;
    }
}
