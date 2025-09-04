using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class NextPreditedOrder
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime LastOrderDate { get; set; }
        public DateTime? NextPredictedOrder { get; set; }
    }
}
