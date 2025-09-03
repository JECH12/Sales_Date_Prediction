using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[PrimaryKey("Orderid", "Productid")]
[Table("OrderDetails", Schema = "Sales")]
[Index("Orderid", Name = "idx_nc_orderid")]
[Index("Productid", Name = "idx_nc_productid")]
public partial class OrderDetail
{
    [Key]
    [Column("orderid")]
    public int Orderid { get; set; }

    [Key]
    [Column("productid")]
    public int Productid { get; set; }

    [Column("unitprice", TypeName = "money")]
    public decimal Unitprice { get; set; }

    [Column("qty")]
    public short Qty { get; set; }

    [Column("discount", TypeName = "numeric(4, 3)")]
    public decimal Discount { get; set; }

    [ForeignKey("Orderid")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("Productid")]
    [InverseProperty("OrderDetails")]
    public virtual Product Product { get; set; } = null!;
}
