using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Entities.Context;

public partial class StoreSampleContext : DbContext
{
    public StoreSampleContext()
    {
    }

    public StoreSampleContext(DbContextOptions<StoreSampleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CustOrder> CustOrders { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderTotalsByYear> OrderTotalsByYears { get; set; }

    public virtual DbSet<OrderValue> OrderValues { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public DbSet<NextPreditedOrder> NextPredictedOrders { get; set; }

    public DbSet<ClientOrder> ClientOrders { get; set; }

    public DbSet<AllEmployees> AllEmployees { get; set; }

    public DbSet<AllShippers> AllShippers { get; set; }
    public DbSet<AllProducts> AllProducts { get; set; }

    public DbSet<CreateOrderResult> CreateOrderResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:StoreSample");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NextPreditedOrder>().HasNoKey().ToView(null);
        modelBuilder.Entity<ClientOrder>().HasNoKey().ToView(null);
        modelBuilder.Entity<AllEmployees>().HasNoKey().ToView(null);
        modelBuilder.Entity<AllShippers>().HasNoKey().ToView(null);
        modelBuilder.Entity<AllProducts>().HasNoKey().ToView(null);
        modelBuilder.Entity<CreateOrderResult>().HasNoKey().ToView(null);

        modelBuilder.Entity<CustOrder>(entity =>
        {
            entity.ToView("CustOrders", "Sales");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(d => d.Mgr).WithMany(p => p.InverseMgr).HasConstraintName("FK_Employees_Employees");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.Cust).WithMany(p => p.Orders).HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.Emp).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Employees");

            entity.HasOne(d => d.Shipper).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Shippers");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.Qty).HasDefaultValue((short)1);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Products");
        });

        modelBuilder.Entity<OrderTotalsByYear>(entity =>
        {
            entity.ToView("OrderTotalsByYear", "Sales");
        });

        modelBuilder.Entity<OrderValue>(entity =>
        {
            entity.ToView("OrderValues", "Sales");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Suppliers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
