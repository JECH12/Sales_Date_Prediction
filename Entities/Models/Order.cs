using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("Orders", Schema = "Sales")]
[Index("Custid", Name = "idx_nc_custid")]
[Index("Empid", Name = "idx_nc_empid")]
[Index("Orderdate", Name = "idx_nc_orderdate")]
[Index("Shippeddate", Name = "idx_nc_shippeddate")]
[Index("Shipperid", Name = "idx_nc_shipperid")]
[Index("Shippostalcode", Name = "idx_nc_shippostalcode")]
public partial class Order
{
    [Key]
    [Column("orderid")]
    public int Orderid { get; set; }

    [Column("custid")]
    public int? Custid { get; set; }

    [Column("empid")]
    public int Empid { get; set; }

    [Column("orderdate", TypeName = "datetime")]
    public DateTime Orderdate { get; set; }

    [Column("requireddate", TypeName = "datetime")]
    public DateTime Requireddate { get; set; }

    [Column("shippeddate", TypeName = "datetime")]
    public DateTime? Shippeddate { get; set; }

    [Column("shipperid")]
    public int Shipperid { get; set; }

    [Column("freight", TypeName = "money")]
    public decimal Freight { get; set; }

    [Column("shipname")]
    [StringLength(40)]
    public string Shipname { get; set; } = null!;

    [Column("shipaddress")]
    [StringLength(60)]
    public string Shipaddress { get; set; } = null!;

    [Column("shipcity")]
    [StringLength(15)]
    public string Shipcity { get; set; } = null!;

    [Column("shipregion")]
    [StringLength(15)]
    public string? Shipregion { get; set; }

    [Column("shippostalcode")]
    [StringLength(10)]
    public string? Shippostalcode { get; set; }

    [Column("shipcountry")]
    [StringLength(15)]
    public string Shipcountry { get; set; } = null!;

    [ForeignKey("Custid")]
    [InverseProperty("Orders")]
    public virtual Customer? Cust { get; set; }

    [ForeignKey("Empid")]
    [InverseProperty("Orders")]
    public virtual Employee Emp { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("Shipperid")]
    [InverseProperty("Orders")]
    public virtual Shipper Shipper { get; set; } = null!;
}
