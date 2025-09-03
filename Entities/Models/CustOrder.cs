using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Keyless]
public partial class CustOrder
{
    [Column("custid")]
    public int? Custid { get; set; }

    [Column("ordermonth", TypeName = "datetime")]
    public DateTime? Ordermonth { get; set; }

    [Column("qty")]
    public int? Qty { get; set; }
}
