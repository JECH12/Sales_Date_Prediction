using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public short Qty { get; set; }
        public float Discount { get; set; }
    }
}
