using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Keyless]
public partial class OrderValue
{
    [Column("orderid")]
    public int Orderid { get; set; }

    [Column("custid")]
    public int? Custid { get; set; }

    [Column("empid")]
    public int Empid { get; set; }

    [Column("shipperid")]
    public int Shipperid { get; set; }

    [Column("orderdate", TypeName = "datetime")]
    public DateTime Orderdate { get; set; }

    [Column("val", TypeName = "numeric(12, 2)")]
    public decimal? Val { get; set; }
}
