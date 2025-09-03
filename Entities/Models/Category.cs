using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("Categories", Schema = "Production")]
[Index("Categoryname", Name = "categoryname")]
public partial class Category
{
    [Key]
    [Column("categoryid")]
    public int Categoryid { get; set; }

    [Column("categoryname")]
    [StringLength(15)]
    public string Categoryname { get; set; } = null!;

    [Column("description")]
    [StringLength(200)]
    public string Description { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
