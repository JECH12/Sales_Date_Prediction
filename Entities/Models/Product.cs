using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("Products", Schema = "Production")]
[Index("Categoryid", Name = "idx_nc_categoryid")]
[Index("Productname", Name = "idx_nc_productname")]
[Index("Supplierid", Name = "idx_nc_supplierid")]
public partial class Product
{
    [Key]
    [Column("productid")]
    public int Productid { get; set; }

    [Column("productname")]
    [StringLength(40)]
    public string Productname { get; set; } = null!;

    [Column("supplierid")]
    public int Supplierid { get; set; }

    [Column("categoryid")]
    public int Categoryid { get; set; }

    [Column("unitprice", TypeName = "money")]
    public decimal Unitprice { get; set; }

    [Column("discontinued")]
    public bool Discontinued { get; set; }

    [ForeignKey("Categoryid")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("Supplierid")]
    [InverseProperty("Products")]
    public virtual Supplier Supplier { get; set; } = null!;
}
